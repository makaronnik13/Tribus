using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayer : Photon.MonoBehaviour
{
    public Player player;

    public void EndTurn()
    {
        NetworkCardGameManager.sInstance.EndTurn();
    }
}
