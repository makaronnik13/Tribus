using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class NetworkCardGameManager : Photon.MonoBehaviour
{
    public class ServerPlayer
    {
		public ServerPlayer(string playerName, float[] playerColor, int v, PhotonPlayer player, string[] cards)
        {
            this.playerName = playerName;
            this.playerColor = playerColor;
            this.spriteId = v;
            this.photonPlayer = player;
            this.cardsIds = cards;
        }

        public string playerName;
        public float[] playerColor;
        public int spriteId;
        public PhotonPlayer photonPlayer;
		public string[] cardsIds;
    }

    public void Mutate(Block block, CombineModel.Skills evolveType, int evolveLevel)
    { 
            CellState cs = block.State.CombinationResult(evolveType, evolveLevel);
            if (cs)
            {
                ChangeState(block, cs);
                ChangeOwner(block, PhotonNetwork.player);
            }
    }

    public void ChangeOwner(Block block, PhotonPlayer player)
    {
        block.photonView.RPC("RpcChangeOwner", PhotonTargets.All, new object[] {player});
    }

    public void ChangeBiom(Block block, CombineModel.Biom biom)
    {
        block.photonView.RPC("RpcChangeBiom", PhotonTargets.All, new object[] {biom});
    }

    public void ChangeState(Block block, CellState state)
    {
        block.photonView.RPC("RpcChangeState", PhotonTargets.All, new object[] {DefaultResourcesManager.AllStatesList.States.IndexOf(state)});
    }

    static public NetworkCardGameManager sInstance = null;
    private Queue<ServerPlayer> playersQueue = new Queue<ServerPlayer>();

    public ServerPlayer CurrentPlayer;

    public Color GetPlayerColor(PhotonPlayer player)
    {
        foreach (PlayerVisual pv in FindObjectsOfType<PlayerVisual>())
        {
            if (pv.Player == player)
            {
                return pv.Color;
            }
        }

        return Color.white;
    }

    public string GetPlayerName(PhotonPlayer player)
    {
        foreach (PlayerVisual pv in FindObjectsOfType<PlayerVisual>())
        {
            if (pv.Player == player)
            {
                return pv.PlayerName;
            }
        }

        return "";
    }
    [PunRPC]
	public void AddPlayer(string playerName, float[] playerColor, int v, PhotonPlayer player, string[] cards)
    {
        playersQueue.Enqueue(new ServerPlayer(playerName, playerColor, v, player, cards));
        if (PhotonNetwork.playerList.Count() == playersQueue.Count)
        {
            StartGame();
        }
    }

	public void CreatePlayer(string playerName, Color playerColor, int v, PhotonPlayer player, string[] cards)
    {
        GameObject lobbyPlayer = PhotonNetwork.Instantiate("PhotonGamePlayer", Vector3.zero, Quaternion.identity, 0, new object[0]);
        lobbyPlayer.GetComponent<PhotonView>().TransferOwnership(player.ID);
        lobbyPlayer.GetComponent<PlayerVisual>().Init(playerName, new float[3] { playerColor.r, playerColor.g, playerColor.b}, v, cards.ToList().OrderBy(c=>Guid.NewGuid()).ToArray(), player == playersQueue.ToList()[0].photonPlayer);
    }

    private void StartGame()
    {
        playersQueue = new Queue<ServerPlayer>(playersQueue.ToList().OrderBy(p=>Guid.NewGuid()));

        foreach (ServerPlayer pp in playersQueue.Reverse())
        {
            CreatePlayer(pp.playerName,  new Color(pp.playerColor[0], pp.playerColor[1], pp.playerColor[2], 1), pp.spriteId, pp.photonPlayer, pp.cardsIds);
        }

        StartCoroutine(CreatePlayers(2f));
            
        BlocksField.Instance.GetComponent<PhotonView>().RPC("GenerateField", PhotonTargets.MasterClient, new object[0]);
    }

    private IEnumerator CreatePlayers(float v)
    {
        yield return new WaitForSeconds(v);
        ServerPlayer currentPlayer = playersQueue.Dequeue();
        foreach (ServerPlayer sp in playersQueue)
        {
            for (int i = 0; i < 5; i++)
            {

                PlayerGetCard(sp.photonPlayer);
            }
        }
        GetComponent<PhotonView>().RPC("SetActivePlayer", PhotonTargets.All, new object[5] { currentPlayer.photonPlayer, currentPlayer.playerName, currentPlayer.playerColor, currentPlayer.spriteId, currentPlayer.cardsIds });
        GetComponent<PhotonView>().RPC("StartPlayerTurn", currentPlayer.photonPlayer, new object[0]);
    }

    [PunRPC]
	private void SetActivePlayer(PhotonPlayer player, string playerName, float[] playerColor, int v, string[] cards)
    {
        CurrentPlayer = new ServerPlayer(playerName, playerColor, v, player, cards);
    }

    void Awake()
    {
        sInstance = this;
    }

    public void EndTurn()
    {
        GetComponent<PhotonView>().RPC("EndTurnOnServer", PhotonTargets.MasterClient, new object[0]);
    }

    [PunRPC]
    private void StartPlayerTurn()
    {
        LocalPlayerLogic.Instance.StartTurn();
    }

    [PunRPC]
    private void EndPlayerTurn()
    {
        LocalPlayerLogic.Instance.EndTurn();
    }

    [PunRPC]
    public void EndTurnOnServer()
    { 
        playersQueue.Enqueue(CurrentPlayer);
        GetComponent<PhotonView>().RPC("EndPlayerTurn", CurrentPlayer.photonPlayer, new object[0]);
        CurrentPlayer = playersQueue.Dequeue();
        GetComponent<PhotonView>().RPC("SetActivePlayer", PhotonTargets.All, new object[5] { CurrentPlayer.photonPlayer, CurrentPlayer.playerName, CurrentPlayer.playerColor, CurrentPlayer.spriteId, CurrentPlayer.cardsIds });
        GetComponent<PhotonView>().RPC("RpcSetActivePlayerVisual", PhotonTargets.All, new object[0]);
        GetComponent<PhotonView>().RPC("StartPlayerTurn", CurrentPlayer.photonPlayer, new object[0]);
    }

    [PunRPC]
    public void RpcSetActivePlayerVisual()
    {
        PlayersVisualizer.Instance.SetActivePlayer();
    }

    //player actions
    public void PlayerStartTurn(PhotonPlayer pp)
    {
        int cardsInHand = GetPlayerHand(pp).Count;
        if (cardsInHand<4)
        {         
            for (int i = 0; i<4 - cardsInHand;i++)
            {

                PlayerGetCard(pp);
            }
        }
        PlayerGetCard(pp);
    }

    public void PlayerGetCard(PhotonPlayer pp)
    {
        //if pile is over, then shuffle drop into pile
        if (GetPlayerPile(pp).Count == 0)
        {
            ReshuflePile(pp);
        }

        //if pile is still over, don't take card
        if (GetPlayerPile(pp).Count == 0)
        {
            return;
        }

        Card c = GetPlayerPile(pp)[0];

        RemoveCardFromPile(c, pp);
        AddCardToHand(c , pp, LocalPlayerVisual.CardAnimationAim.Pile);
    }

    public void ReshuflePile(PhotonPlayer pp)
    {
        List<Card> drop = GetPlayerDrop(pp);
		List<string> cardsIds = new List<string>();

        drop = drop.OrderBy(c => Guid.NewGuid()).ToList();

        foreach (Card c in drop)
        {
			cardsIds.Add(c.name);
        }

        ClearDrop(pp);
        SetPile(drop, pp);
    }

    private void SetPile(List<Card> cards, PhotonPlayer player)
    {
        foreach (Card card in cards)
        {
            AddCardToPile(card, player);
        }
    }

    private void ClearDrop(PhotonPlayer pp)
    {
        foreach (Card card in GetPlayerDrop(pp))
        {
            RemoveCardFromDrop(card, pp);
        }
    }

    public void PlayerDropCard(Card c, PhotonPlayer pp)
    {

    }

    //cards changes
    public void AddCardToDrop(Card card, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim = LocalPlayerVisual.CardAnimationAim.Top, bool animate = true)
    {
        AddCardsToDrop(new List<Card> { card }, player, aim, animate);
    }

    public void AddCardsToDrop(List<Card> cards, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim, bool animate = true)
    {
        string[] cardsNames = cards.Select(c => c.name).ToArray();
        GetComponent<PhotonView>().RPC("RpcAddCardsToDrop", PhotonTargets.All, new object[] { cardsNames, player, aim, animate});
    }


    [PunRPC]
	private void RpcAddCardsToDrop(string[] cardsIds, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim, bool animate = false)
    {
        if (player == PhotonNetwork.player && animate)
        {
            LocalPlayerVisual.Instance.AddCardsToDrop(cardsIds.ToList(), (CardVisual visual) =>
            {

            }, aim);
        }
        FindObjectsOfType<PlayerVisual>().Where(v => v.Player == player).ToList()[0].AddCardsToDrop(cardsIds);
    }

    public void AddCardToPile(Card card, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim = LocalPlayerVisual.CardAnimationAim.Top)
    {
        AddCardsToPile(new List<Card> { card }, player, aim);
    }

    public void AddCardsToPile(List<Card> cards, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim)
    {
        string[] cardsNames = cards.Select(c => c.name).ToArray();
        GetComponent<PhotonView>().RPC("RpcAddCardsToPile", PhotonTargets.All, new object[] {cardsNames, player, aim});
    }

    [PunRPC]
	private void RpcAddCardsToPile(string[]  cardsIds, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim)
    {
        if (player == PhotonNetwork.player)
        {
            LocalPlayerVisual.Instance.AddCardsToPile(cardsIds.ToList(), (CardVisual visual) =>
            {

            }, aim);
        }
        FindObjectsOfType<PlayerVisual>().Where(v => v.Player == player).ToList()[0].AddCardsToPile(cardsIds);
    }

    public void AddCardToHand(Card card, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim = LocalPlayerVisual.CardAnimationAim.Top)
    {
        AddCardsToHand(new List<Card> {card}, player, aim);
    }

    public void AddCardsToHand(List<Card> cards, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim)
    {
        string[] cardsNames = cards.Select(c=>c.name).ToArray();
        GetComponent<PhotonView>().RPC("RpcAddCardsToHand", PhotonTargets.All, new object[] { cardsNames, player, aim});
    }


    [PunRPC]
    private void RpcAddCardsToHand(string[] cardsIds, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim)
    {

        if (player == PhotonNetwork.player)
        {
            LocalPlayerVisual.Instance.AddCardsToHand(cardsIds.ToList(), (CardVisual visual) =>
        {

        }, aim);   
        }

        FindObjectsOfType<PlayerVisual>().Where(v => v.Player == player).ToList()[0].AddCardsToHand(cardsIds);
    }

    public void RemoveCardFromDrop(Card card, PhotonPlayer player)
    {
		GetComponent<PhotonView>().RPC("RpcRemoveCardFromDrop", PhotonTargets.All, new object[] {card.name, player });
    }

    [PunRPC]
	private void RpcRemoveCardFromDrop(string  cardId, PhotonPlayer player)
    {
        FindObjectsOfType<PlayerVisual>().Where(v => v.Player == player).ToList()[0].RemoveCardFromDrop(cardId);
    }

    public void RemoveCardFromPile(Card card, PhotonPlayer player)
    {
		GetComponent<PhotonView>().RPC("RpcRemoveCardFromPile", PhotonTargets.All, new object[] { card.name, player });
    }

    [PunRPC]
	private void RpcRemoveCardFromPile(string  cardId, PhotonPlayer player)
    {
        FindObjectsOfType<PlayerVisual>().Where(v=>v.Player == player).ToList()[0].RemoveCardFromPile(cardId);
    }


    public void RemoveCardFromHand(Card card, PhotonPlayer player)
    {
		GetComponent<PhotonView>().RPC("RpcRemoveCardFromHand", PhotonTargets.All, new object[] {card.name, player });
    }

    [PunRPC]
	private void RpcRemoveCardFromHand(string cardId, PhotonPlayer player)
    {
        FindObjectsOfType<PlayerVisual>().Where(v => v.Player == player).ToList()[0].RemoveCardFromHand(cardId);
    }

    public void RemoveCardFromPlayer(Card card, PhotonPlayer player)
    {

    }

    public void DropCard(Card cardAsset)
    {
        AddCardToDrop(cardAsset, PhotonNetwork.player, LocalPlayerVisual.CardAnimationAim.Drop, false);
        RemoveCardFromHand(cardAsset, PhotonNetwork.player);
    }

    public List<Card> GetPlayerHand(PhotonPlayer pp)
    {
        List<Card> cards = new List<Card>();
        foreach (PlayerVisual pv in FindObjectsOfType<PlayerVisual>())
        {
            if (pv.Player == pp)
            {
				foreach (string cardId in pv.Hand)
                {
					cards.Add(DefaultResourcesManager.GetCardById(cardId));
                }
            }
        }
        return cards;
    }

    public List<Card> GetPlayerPile(PhotonPlayer pp)
    {
        List<Card> cards = new List<Card>();
        foreach (PlayerVisual pv in FindObjectsOfType<PlayerVisual>())
        {
            if (pv.Player == pp)
            {
				foreach (string cardId in pv.Pile)
                {
					cards.Add(DefaultResourcesManager.GetCardById(cardId));
                }
            }
        }
        return cards;
    }

    public List<Card> GetPlayerDrop(PhotonPlayer pp)
    {
        List<Card> cards = new List<Card>();
        foreach (PlayerVisual pv in FindObjectsOfType<PlayerVisual>())
        {
            if (pv.Player == pp)
            {
				foreach (string cardId in pv.Drop)
                {
					cards.Add(DefaultResourcesManager.GetCardById(cardId));
                }
            }
        }
        return cards;
    }

    public List<Card> GetPlayerCards(PhotonPlayer pp)
    {
        List<Card> cards = new List<Card>();
        cards.AddRange(GetPlayerDrop(pp));
        cards.AddRange(GetPlayerHand(pp));
        cards.AddRange(GetPlayerPile(pp));
        return cards;
    }


	public void ActivateSkill(Block aimBlock, CombineModel.Skills skill, int skillLevel)
	{
		CellState cs = aimBlock.State.CombinationResult (skill,skillLevel);
		if(cs)
		{
			aimBlock.photonView.RPC ("RpcChangeOwner", PhotonTargets.All, new object[]{ PhotonNetwork.player});
			aimBlock.photonView.RPC ("RpcChangeState", PhotonTargets.All, new object[]{ DefaultResourcesManager.AllStatesList.States.IndexOf(cs)});
		}
	}
}

