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
        public ServerPlayer(string playerName, float[] playerColor, int v, PhotonPlayer player, int[] cards)
        {
            this.playerName = playerName;
            this.playerColor = playerColor;
            this.spriteId = v;
            this.photonPlayer = player;
        }

        public string playerName;
        public float[] playerColor;
        public int spriteId;
        public PhotonPlayer photonPlayer;
        public int[] cardsIds;
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
    public void AddPlayer(string playerName, float[] playerColor, int v, PhotonPlayer player, int[] cards)
    {
        playersQueue.Enqueue(new ServerPlayer(playerName, playerColor, v, player, cards));
        if (PhotonNetwork.playerList.Count() == playersQueue.Count)
        {
            StartGame();
        }
    }

    public void CreatePlayer(string playerName, Color playerColor, int v, PhotonPlayer player, int[] cards)
    {
        GameObject lobbyPlayer = PhotonNetwork.Instantiate("PhotonGamePlayer", Vector3.zero, Quaternion.identity, 0, new object[0]);
        lobbyPlayer.GetComponent<PhotonView>().TransferOwnership(player.ID);
        lobbyPlayer.GetComponent<PlayerVisual>().Init(playerName, new float[3] { playerColor.r, playerColor.g, playerColor.b}, v, cards.ToList().OrderBy(c=>Guid.NewGuid()).ToArray());
    }

    private void StartGame()
    {
        playersQueue = new Queue<ServerPlayer>(playersQueue.ToList().OrderBy(p=>Guid.NewGuid()));

        foreach (ServerPlayer pp in playersQueue)
        {
            CreatePlayer(pp.playerName,  new Color(pp.playerColor[0], pp.playerColor[1], pp.playerColor[2], 1), pp.spriteId, pp.photonPlayer, pp.cardsIds);
        }

        ServerPlayer currentPlayer = playersQueue.Dequeue();

        GetComponent<PhotonView>().RPC("SetActivePlayer", PhotonTargets.All, new object[5] {currentPlayer.photonPlayer, currentPlayer.playerName, currentPlayer.playerColor, currentPlayer.spriteId, currentPlayer.cardsIds});
        GetComponent<PhotonView>().RPC("StartPlayerTurn", currentPlayer.photonPlayer, new object[0]);      
        BlocksField.Instance.GetComponent<PhotonView>().RPC("GenerateField", PhotonTargets.MasterClient, new object[0]);
    }

    [PunRPC]
    private void SetActivePlayer(PhotonPlayer player, string playerName, float[] playerColor, int v, int[] cards)
    {
        CurrentPlayer = new ServerPlayer(playerName, playerColor, v, player, cards);
        PlayersVisualizer.Instance.SetActivePlayer();
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
        GetComponent<PhotonView>().RPC("UpdatePlayersVisual", PhotonTargets.All, new object[0]);
        playersQueue.Enqueue(CurrentPlayer);
        GetComponent<PhotonView>().RPC("EndPlayerTurn", CurrentPlayer.photonPlayer, new object[0]);
        CurrentPlayer = playersQueue.Dequeue();
        GetComponent<PhotonView>().RPC("StartPlayerTurn", CurrentPlayer.photonPlayer, new object[0]);
    }

    //player actions
    public void PlayerStartTurn(PhotonPlayer pp)
    {
        if (GetPlayerHand(pp).Count<5)
        {
            for (int i = 0; i< 5 - GetPlayerHand(pp).Count;i++)
            {
                PlayerGetCard(pp);
            }
        }
        PlayerGetCard(pp);
    }

    public void PlayerGetCard(PhotonPlayer pp)
    {

    }

    public void PlayerDropCard(Card c, PhotonPlayer pp)
    {

    }

    //cards changes
    public void AddCardToDrop(Card card, PhotonPlayer player)
    {

    }

    public void AddCardToPile(Card card, PhotonPlayer player)
    {

    }

    public void AddCardToHand(Card card, PhotonPlayer player)
    {

    }

    public void RemoveCardFromDrop(Card card, PhotonPlayer player)
    {

    }

    public void RemoveCardFromPile(Card card, PhotonPlayer player)
    {

    }

    public void RemoveCardFromHand(Card card, PhotonPlayer player)
    {

    }

    public void RemoveCardFromPlayer(Card card, PhotonPlayer player)
    {

    }

    public List<Card> GetPlayerHand(PhotonPlayer pp)
    {
        List<Card> cards = new List<Card>();
        foreach (PlayerVisual pv in FindObjectsOfType<PlayerVisual>())
        {
            if (pv.Player == pp)
            {
                foreach (int cardId in pv.Hand)
                {
                    cards.Add(DefaultResourcesManager.AllCards[cardId]);
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
                foreach (int cardId in pv.Pile)
                {
                    cards.Add(DefaultResourcesManager.AllCards[cardId]);
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
                foreach (int cardId in pv.Drop)
                {
                    cards.Add(DefaultResourcesManager.AllCards[cardId]);
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
}

