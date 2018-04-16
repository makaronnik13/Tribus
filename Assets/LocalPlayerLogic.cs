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
            return NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer == PhotonNetwork.player;
        }
    }

    private void Start()
    {
        visual = GetComponent<LocalPlayerVisual>();
        Instance = this;
        float[] playerColor = new float[3] { LobbyPlayerIdentity.Instance.player.PlayerColor.r, LobbyPlayerIdentity.Instance.player.PlayerColor.g, LobbyPlayerIdentity.Instance.player.PlayerColor.b};
        List<int> cardsIds = new List<int>();
        foreach (Card c in LobbyPlayerIdentity.Instance.player.CurrentDeck.Cards)
        {
            cardsIds.Add(DefaultResourcesManager.AllCards.ToList().IndexOf(c));
        }
        NetworkCardGameManager.sInstance.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.MasterClient, new object[] { LobbyPlayerIdentity.Instance.player.PlayerName, playerColor, LobbyPlayerIdentity.Instance.player.PlayerAvatarId, PhotonNetwork.player, cardsIds.ToArray()});
    }

    public void OnEndTurnPush()
    {
        NetworkCardGameManager.sInstance.EndTurn();
    }

    public void EndTurn()
    {
        visual.EndTurn();
        ResourcesManager.Instance.EndTurn();
    }

    public void StartTurn()
    {
        NetworkCardGameManager.sInstance.PlayerStartTurn(PhotonNetwork.player);
        ResourcesManager.Instance.StartTurn();
        visual.StartTurn();
    }

    public void GetCard(int cardId)
    {
        visual.GetCard(DefaultResourcesManager.AllCards[cardId]);
    }

}
