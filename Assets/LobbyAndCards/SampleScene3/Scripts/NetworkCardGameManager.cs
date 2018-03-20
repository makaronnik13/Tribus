using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;

public class NetworkCardGameManager : NetworkBehaviour
{
    static public NetworkCardGameManager sInstance = null;
    protected bool _running = true;

    void Awake()
    {
        sInstance = this;
    }

    void Start()
    {

    }

    [ServerCallback]
    void Update()
    {
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        /*
        foreach (GameObject obj in asteroidPrefabs)
        {
            ClientScene.RegisterPrefab(obj);
        }*/
    }

    IEnumerator ReturnToLoby()
    {
        _running = false;
        yield return new WaitForSeconds(3.0f);
        LobbyManager.s_Singleton.ServerReturnToLobby();
    }

}
