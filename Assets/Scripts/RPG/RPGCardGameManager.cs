﻿using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class RPGCardGameManager : Photon.MonoBehaviour
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

    public void AddModifier(WarriorObject p, Effect addingEffect, float time)
    {
        p.AddModifier(addingEffect, time);
    }

    public void Block(WarriorObject p, int value)
    {
        PlayersWarrior(PhotonNetwork.player).Animate(() =>
        {
            p.GetBlock(value);
        });
    }

    public void Damage(WarriorObject p, int value)
    {
        PlayersWarrior(PhotonNetwork.player).DealDamage(p, value);
    }

    private WarriorObject PlayersWarrior(PhotonPlayer player)
    {
        foreach (WarriorObject w in BattleField.Instance.Players)
        {
            if (w.Player == player)
            {
                return w;
            }
        }
        return null;
    }

    static public RPGCardGameManager sInstance = null;

	private Queue<ServerPlayer> playersQueue = new Queue<ServerPlayer> ();
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

		StartBattle ();
	}
		
	private void StartBattle()
	{
		foreach (ServerPlayer pp in playersQueue.Reverse()) 
		{
			for (int i = 0; i < 5; i++) 
			{
				PlayerGetCard (pp.photonPlayer);
			}
		}

		BattleField.Instance.StartBattle (FindObjectsOfType<PlayerVisual>().ToList());
	}

	public void StartPlayerTurn(PhotonPlayer player)
	{
		ServerPlayer currentPlayer = playersQueue.ToList().FirstOrDefault(p=>p.photonPlayer==player);
		GetComponent<PhotonView>().RPC("SetActivePlayer", PhotonTargets.All, new object[5] { currentPlayer.photonPlayer, currentPlayer.playerName, currentPlayer.playerColor, currentPlayer.spriteId, currentPlayer.cardsIds });
		GetComponent<PhotonView>().RPC("StartPlayerTurn", currentPlayer.photonPlayer, new object[0]);
	}

	[PunRPC]
	private void SetActivePlayer(PhotonPlayer player, string playerName, float[] playerColor, int v, string[] cards)
	{
		CurrentPlayer = new ServerPlayer(playerName, playerColor, v, player, cards);
		foreach(CardVisual c in CardsManager.Instance.HandCardsLayout.Cards)
		{
			c.UpdateAvaliablility ();
		}
	}

	void Awake()
	{
		sInstance = this;
	}

	public void EndTurn()
	{
        AddCardsToDrop(GetPlayerHand(PhotonNetwork.player), PhotonNetwork.player, LocalPlayerVisual.CardAnimationAim.Hand, true, true);
        RemoveCardsFromHand(GetPlayerHand(PhotonNetwork.player), PhotonNetwork.player);

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
		GetComponent<PhotonView>().RPC("EndPlayerTurn", CurrentPlayer.photonPlayer, new object[0]);
		InitiativeTimeline.Instance.StartTimeline ();
	}

	[PunRPC]
	public void RpcSetActivePlayerVisual()
	{
		//PlayersVisualizer.Instance.SetActivePlayer();
	}

	//player actions
	public void PlayerStartTurn(PhotonPlayer pp)
	{
			for (int i = 0; i<5;i++)
			{

				PlayerGetCard(pp);
			}
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
		AddCardToHand(c , pp, LocalPlayerVisual.CardAnimationAim.Pile, true);
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


	//cards changes
	public void AddCardToDrop(Card card, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim = LocalPlayerVisual.CardAnimationAim.Top, bool animate = true, bool dontWait = false)
	{
		AddCardsToDrop(new List<Card> { card }, player, aim, animate,dontWait);
	}

	public void AddCardsToDrop(List<Card> cards, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim, bool animate = true, bool dontWait = false)
	{
		string[] cardsNames = cards.Select(c => c.name).ToArray();
		GetComponent<PhotonView>().RPC("RpcAddCardsToDrop", PhotonTargets.AllBuffered, new object[] { cardsNames, player, aim, animate, dontWait});
	}


	[PunRPC]
	private void RpcAddCardsToDrop(string[] cardsIds, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim, bool animate = false, bool dontWait = false)
	{
		if (player == PhotonNetwork.player && animate)
		{
			LocalPlayerVisual.Instance.AddCardsToDrop(cardsIds.ToList(), (CardVisual visual) =>
				{

				}, aim, dontWait);
		}
		FindObjectsOfType<PlayerVisual>().Where(v => v.Player == player).ToList()[0].AddCardsToDrop(cardsIds);
	}

	public void AddCardToPile(Card card, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim = LocalPlayerVisual.CardAnimationAim.Top,  bool animate = false, bool dontWait = false)
	{
		AddCardsToPile(new List<Card> { card }, player, aim, dontWait);
	}

	public void AddCardsToPile(List<Card> cards, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim,  bool animate = false, bool dontWait = false)
	{
		string[] cardsNames = cards.Select(c => c.name).ToArray();
		GetComponent<PhotonView>().RPC("RpcAddCardsToPile", PhotonTargets.AllBuffered, new object[] {cardsNames, player, aim, animate, dontWait});
	}

	[PunRPC]
	private void RpcAddCardsToPile(string[]  cardsIds, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim,  bool animate = false,  bool dontWait = false)
	{
		if (player == PhotonNetwork.player && animate)
		{
			LocalPlayerVisual.Instance.AddCardsToPile(cardsIds.ToList(), (CardVisual visual) =>
				{

				}, aim, dontWait);
		}
		FindObjectsOfType<PlayerVisual>().Where(v => v.Player == player).ToList()[0].AddCardsToPile(cardsIds);
	}

	public void AddCardToHand(Card card, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim = LocalPlayerVisual.CardAnimationAim.Top,  bool animate = false, bool dontWait = false)
	{
		AddCardsToHand(new List<Card> {card}, player, aim, animate, dontWait);
	}

	public void AddCardsToHand(List<Card> cards, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim,  bool animate = false, bool dontWait = false)
	{
		string[] cardsNames = cards.Select(c=>c.name).ToArray();
		GetComponent<PhotonView>().RPC("RpcAddCardsToHand", PhotonTargets.AllBuffered, new object[] { cardsNames, player, aim, animate, dontWait});
	}


	[PunRPC]
	private void RpcAddCardsToHand(string[] cardsIds, PhotonPlayer player, LocalPlayerVisual.CardAnimationAim aim,  bool animate = false, bool dontWait = false)
	{

		if (player == PhotonNetwork.player && animate)
		{
			LocalPlayerVisual.Instance.AddCardsToHand(cardsIds.ToList(), (CardVisual visual) =>
				{

				}, aim, dontWait);   
		}

		FindObjectsOfType<PlayerVisual>().Where(v => v.Player == player).ToList()[0].AddCardsToHand(cardsIds);
	}

	public void RemoveCardFromDrop(Card card, PhotonPlayer player, bool animate = false)
	{
		RemoveCardsFromDrop (new List<Card>(){card}, player, animate);
	}

	public void RemoveCardsFromDrop(List<Card> cards, PhotonPlayer player, bool animate = false, bool burn = true)
	{
		GetComponent<PhotonView>().RPC("RpcRemoveCardsFromDrop", PhotonTargets.AllBuffered, new object[] {cards.Select(c=>c.name).ToArray(), player, animate, burn});
	}

	[PunRPC]
	private void RpcRemoveCardsFromDrop(string[] cardsIds, PhotonPlayer player, bool animate = false, bool burn = true)
	{
		if(player == PhotonNetwork.player && animate)
		{
			if (burn)
			{
				LocalPlayerVisual.Instance.BurnCardsFromDrop(cardsIds);
			}
			else
			{
				LocalPlayerVisual.Instance.SteelCardsFromDrop(cardsIds);
			}
		}
		FindObjectsOfType<PlayerVisual>().Where(v => v.Player == player).ToList()[0].RemoveCardsFromDrop(cardsIds.ToList());
	}

	public void RemoveCardFromPile(Card card, PhotonPlayer player, bool animate = false)
	{
		RemoveCardsFromPile (new List<Card>(){card}, player, animate);
	}

	public void RemoveCardsFromPile(List<Card> cards, PhotonPlayer player, bool animate = false, bool burn = true)
	{
		GetComponent<PhotonView>().RPC("RpcRemoveCardsFromPile", PhotonTargets.AllBuffered, new object[] { cards.Select(c=>c.name).ToArray(), player,animate, burn });
	}

	[PunRPC]
	private void RpcRemoveCardsFromPile(string[]  cardsIds, PhotonPlayer player, bool animate = false, bool burn = true)
	{
		if(player == PhotonNetwork.player && animate)
		{
			if (burn)
			{
				LocalPlayerVisual.Instance.BurnCardsFromPile(cardsIds);
			}
			else
			{
				LocalPlayerVisual.Instance.SteelCardsFromPile(cardsIds);
			}
		}
		FindObjectsOfType<PlayerVisual>().Where(v=>v.Player == player).ToList()[0].RemoveCardsFromPile(cardsIds.ToList());
	}

	public void RemoveCardsFromHand(List<Card> cards, PhotonPlayer player, bool animate = false, bool burn = true)
	{
		GetComponent<PhotonView>().RPC("RpcRemoveCardsFromHand", PhotonTargets.AllBuffered, new object[] {cards.Select(c=>c.name).ToArray(), player, animate, burn});
	}

	public void RemoveCardFromHand(Card card, PhotonPlayer player, bool animate = false)
	{
		RemoveCardsFromHand (new List<Card>(){card}, player, animate);
	}

	[PunRPC]
	private void RpcRemoveCardsFromHand(string[] cardsIds, PhotonPlayer player, bool animate, bool burn = true)
	{
		if(player == PhotonNetwork.player && animate)
		{
			if (burn)
			{
				LocalPlayerVisual.Instance.BurnCardsFromHand(cardsIds);
			}
			else
			{
				LocalPlayerVisual.Instance.SteelCardsFromHand(cardsIds);
			}
		}
		FindObjectsOfType<PlayerVisual>().Where(v => v.Player == player).ToList()[0].RemoveCardsFromHand(cardsIds);
	}

	public void RemoveCardsFromPlayer(List<Card> card, PhotonPlayer player, bool animate = false, bool burn = true)
	{

	}

	public void DropCards(PhotonPlayer player, List<Card> cardsAsset)
	{
		AddCardsToDrop(cardsAsset, player, LocalPlayerVisual.CardAnimationAim.Hand, true);
		RemoveCardsFromHand(cardsAsset, player);
	}

	public void DropCard(PhotonPlayer player, Card cardAsset)
	{
		AddCardToDrop(cardAsset, player, LocalPlayerVisual.CardAnimationAim.Hand, true);
		RemoveCardFromHand(cardAsset, player);
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


}

