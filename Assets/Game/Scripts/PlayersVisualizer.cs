using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayersVisualizer : Singleton<PlayersVisualizer>
{
	private bool firstTime = false;

	public void SetActivePlayer()
	{
		if (firstTime) {
			SetActivePlayerAllTimes ();
		} else 
		{
			SetActivePlayerFirstTime ();
		}
	}

	private void SetActivePlayerFirstTime()
	{
		transform.GetChild (transform.childCount - 1).GetComponent<PlayerVisual> ().SetActivePlayer (true);
		firstTime = true;
	}

	private void SetActivePlayerAllTimes()
	{
		transform.GetChild (transform.childCount - 1).GetComponent<PlayerVisual> ().SetActivePlayer (false);
		transform.GetChild (transform.childCount - 1).GetComponent<PlayerVisual> ().SetActivePlayer (true);
	}
}
