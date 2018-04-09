using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersVisualizer : MonoBehaviour
{

	public GameObject playerPrefab;
	private bool firstTime = false;

	public void Init(List<Player> players)
	{
		players.Reverse ();
		foreach(Player player in players)
		{
			Debug.Log (playerPrefab);
			GameObject newPlayerVisual = Instantiate (playerPrefab);
			newPlayerVisual.transform.SetParent (transform);
			newPlayerVisual.transform.localScale = Vector3.one;
			newPlayerVisual.GetComponent<PlayerVisual> ().Init (player);
			newPlayerVisual.transform.localPosition = Vector3.zero;
			newPlayerVisual.transform.localRotation = Quaternion.identity;
		}
	}

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
