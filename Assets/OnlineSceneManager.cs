using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlineSceneManager: NetworkBehaviour
{
	public GameObject treePrefab;

	public void Start()
	{
		GameObject tree = (GameObject)Instantiate(treePrefab, transform.position, transform.rotation);
		NetworkServer.Spawn(tree);
	}
}
