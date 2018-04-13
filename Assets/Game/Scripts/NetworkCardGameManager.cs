using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class NetworkCardGameManager : Photon.MonoBehaviour
{
    static public NetworkCardGameManager sInstance = null;
    private Queue<PhotonPlayer> playersQueue = new Queue<PhotonPlayer>();
    private PhotonPlayer currentPlayer;
    

    public void CreatePlayer(string playerName, Color playerColor, int v, PhotonPlayer player)
    {
        GameObject lobbyPlayer = PhotonNetwork.Instantiate("PhotonGamePlayer", Vector3.zero, Quaternion.identity, 0, new object[0]);
        lobbyPlayer.GetComponent<PhotonView>().TransferOwnership(player.ID);
        lobbyPlayer.GetComponent<PlayerVisual>().Init(new float[3] { playerColor.r, playerColor.g, playerColor.b}, v);
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
    public void EndTurnOnServer()
    {
        //Debug.Log(currentPlayer.player.PlayerName + " ends turn");
        //playersQueue.Enqueue(currentPlayer);
       // currentPlayer.EndTurn(currentPlayer.connectionToClient); 
        //currentPlayer = playersQueue.Dequeue();
        //Debug.Log(currentPlayer.player.PlayerName+" start turn");
       // currentPlayer.StartTurn(currentPlayer.connectionToClient);
    }
}
