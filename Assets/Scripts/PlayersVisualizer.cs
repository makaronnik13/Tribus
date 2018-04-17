using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayersVisualizer : Singleton<PlayersVisualizer>
{
	public void SetActivePlayer()
	{
		transform.GetChild (transform.childCount - 1).GetComponent<PlayerVisual> ().SetActivePlayer (false);
		transform.GetChild (transform.childCount - 1).GetComponent<PlayerVisual> ().SetActivePlayer (true);
	}
}
