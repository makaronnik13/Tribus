using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayerIdentity : NetworkBehaviour
{
    public Player player;

    public void Init(Queue<GamePlayerIdentity> players)
    {


       // GetComponentInChildren<PlayersVisualizer>().Init();
    }
}
