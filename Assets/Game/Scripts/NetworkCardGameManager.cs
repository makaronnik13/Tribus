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

    void Awake()
    {
        sInstance = this;
    }

    [Server]
    private void OnAllClientsConnected()
    {
        Debug.Log("all clients connected");
        playersQueue = new Queue<GamePlayer>(playersQueue.ToList().OrderBy(p=>Guid.NewGuid()));
        List<GameObject> go = new List<GameObject>();
        foreach (GamePlayer gp in playersQueue)
        {
            go.Add(gp.gameObject);
        }

        foreach (GamePlayer gp in playersQueue)
        {
            gp.TargetSetPlayersList(gp.connectionToClient, go.ToArray());
        }

        currentPlayer = playersQueue.Dequeue();
        currentPlayer.TargetStartTurn(currentPlayer.connectionToClient);
    }

    [ServerCallback]
    public void Connected(GameObject playerGo)
    {
        Debug.Log(playerGo.GetComponent<GamePlayer>().player.PlayerName+" connected");
        playersQueue.Enqueue(playerGo.GetComponent<GamePlayer>());

        Debug.Log(playersQueue.Count+"/" + LobbyManager.s_Singleton.matchSize);
        if (playersQueue.Count == LobbyManager.s_Singleton.matchSize)
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
