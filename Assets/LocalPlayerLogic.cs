using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalPlayerLogic : MonoBehaviour {
    public LocalPlayerVisual visual;
    public static LocalPlayerLogic Instance;

    private void Start()
    {
        visual = GetComponent<LocalPlayerVisual>();
        Instance = this;
        NetworkCardGameManager.sInstance.CreatePlayer(LobbyPlayerIdentity.Instance.player.PlayerName, LobbyPlayerIdentity.Instance.player.PlayerColor, DefaultResourcesManager.Avatars.ToList().IndexOf(LobbyPlayerIdentity.Instance.player.PlayerAvatar), PhotonNetwork.player);
    }

    public void OnEndTurnPush()
    {
        NetworkCardGameManager.sInstance.EndTurn();
    }

    public void EndTurn()
    {
        Debug.Log("local logic turn end");
        visual.EndTurn();
    }

    public void StartTurn()
    {
        Debug.Log("local logic turn start");
        visual.StartTurn();
    }

}
