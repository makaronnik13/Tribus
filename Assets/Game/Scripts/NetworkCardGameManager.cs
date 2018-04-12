using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class NetworkCardGameManager : NetworkBehaviour
{
    static public NetworkCardGameManager sInstance = null;

    private Queue<GamePlayer> playersQueue = new Queue<GamePlayer>();
    private GamePlayer currentPlayer;
    private CardGameVisual visual;

    void Awake()
    {
        sInstance = this;
        visual = GetComponent<CardGameVisual>();
    }

    [Server]
    private void OnAllClientsConnected()
    {
        playersQueue = new Queue<GamePlayer>(playersQueue.ToList().OrderBy(p=>Guid.NewGuid()));
        List<GameObject> go = new List<GameObject>();
        //visual.SetPlayersList(playersQueue.ToArray());
        currentPlayer = playersQueue.Dequeue();
        currentPlayer.TargetStartTurn(currentPlayer.connectionToClient);
    }


    [ServerCallback]
    public void Connected(GameObject playerGo)
    {
        playersQueue.Enqueue(playerGo.GetComponent<GamePlayer>());
        UIDebug.Instance.Log(playersQueue.Count().ToString()+"/"+ LobbyManager.s_Singleton.RoomSize);
        if (playersQueue.Count == LobbyManager.s_Singleton.RoomSize) //|| LobbyManager.s_Singleton.RoomSize == 2
        {
            OnAllClientsConnected();
        }
    }

    public void EndTurn()
    {
        Debug.Log(currentPlayer.player.PlayerName + " ends turn");
        playersQueue.Enqueue(currentPlayer);
        currentPlayer.TargetEndTurn(currentPlayer.connectionToClient); 
        currentPlayer = playersQueue.Dequeue();
        Debug.Log(currentPlayer.player.PlayerName+" start turn");
        currentPlayer.TargetStartTurn(currentPlayer.connectionToClient);
    }
}
