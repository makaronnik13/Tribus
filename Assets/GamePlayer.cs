using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour
{
    public Player player;

    public override void OnStartLocalPlayer()
    {
        LobbyMenu.Instance.HideBackGround();
        LocalPlayerLogic.Instance.player = this;
        CmdConnected();
    }


    [Command]
    private void CmdConnected()
    {
        NetworkCardGameManager.sInstance.Connected(gameObject);
    }

    public void EndTurn()
    {
        CmdEndTurn();
    }


    [Command]
    private void CmdEndTurn()
    {
        NetworkCardGameManager.sInstance.EndTurn();
    }

    [TargetRpc]
    public void TargetStartTurn(NetworkConnection connection)
    {
        LocalPlayerLogic.Instance.StartTurn();
    }

    [TargetRpc]
    public void TargetEndTurn(NetworkConnection connection)
    {
        LocalPlayerLogic.Instance.EndTurn();
    }
}
