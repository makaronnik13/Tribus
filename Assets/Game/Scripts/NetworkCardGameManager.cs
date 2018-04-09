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

    private Queue<GamePlayerIdentity> playersQueue;

    void Awake()
    {
        sInstance = this;
    }

    [ServerCallback]
    public void Start()
    {
        InitPlayers();
    }

    private void InitPlayers()
    {
        List<GamePlayerIdentity> newPlayers = new List<GamePlayerIdentity>();
        foreach (GamePlayerIdentity gpi in FindObjectsOfType<GamePlayerIdentity>())
        {
            newPlayers.Add(gpi);
        }
        playersQueue = new Queue<GamePlayerIdentity>(newPlayers.OrderBy(a => Guid.NewGuid()));

        foreach (GamePlayerIdentity gpi in playersQueue)
        {
            gpi.Init(playersQueue);
        }
    }

}
