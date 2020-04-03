using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalPlayerLogic : MonoBehaviour {
    public LocalPlayerVisual visual;
    public static LocalPlayerLogic Instance;
    public bool MyTurn
    {
        get
        {
			return RPGCardGameManager.sInstance.CurrentPlayer!=null && RPGCardGameManager.sInstance.CurrentPlayer.photonPlayer == PhotonNetwork.player;
        }
    }

    private void Start()
    {
        visual = GetComponent<LocalPlayerVisual>();
        Instance = this;
        float[] playerColor = new float[3] { LobbyPlayerIdentity.Instance.player.PlayerColor.r, LobbyPlayerIdentity.Instance.player.PlayerColor.g, LobbyPlayerIdentity.Instance.player.PlayerColor.b};
		List<string> cardsIds = new List<string>();
        foreach (Card c in LobbyPlayerIdentity.Instance.player.CurrentDeck)
        {
			cardsIds.Add(c.name);
        }
		RPGCardGameManager.sInstance.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.MasterClient, new object[] { LobbyPlayerIdentity.Instance.player.PlayerName, playerColor, LobbyPlayerIdentity.Instance.player.PlayerAvatarId, PhotonNetwork.player, cardsIds.ToArray()});
    }

    public void OnEndTurnPush()
    {
		RPGCardGameManager.sInstance.EndTurn();
    }

    public void EndTurn()
    {
        visual.EndTurn();
        ResourcesManager.Instance.EndTurn();
    }

    public void StartTurn()
    {
		RPGCardGameManager.sInstance.PlayerStartTurn(PhotonNetwork.player);
        ResourcesManager.Instance.StartTurn();
        visual.StartTurn();
    }

	public void GetCard(string  cardId)
    {
		visual.GetCard(DefaultResourcesManager.GetCardById(cardId));
    }

}
