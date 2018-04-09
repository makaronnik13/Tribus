using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Prototype.NetworkLobby;

public class RoomCreatingPanel : MonoBehaviour {

	public LobbyManager lobbyManager;

	public TextMeshProUGUI counter;
	public TMP_InputField roomName;

	private int playersInRoom = 2;
	private int PlayersInRoom
	{
		get
		{
			return playersInRoom;
		}
		set
		{
			playersInRoom = Mathf.Clamp (value, 2, 4);
			counter.text = playersInRoom.ToString();
		}
	}

	public void Create()
	{
		FindObjectOfType<LobbyMainMenu> ().OnClickCreateMatchmakingGame (roomName.text, PlayersInRoom);
		Cancel ();
		LobbyMenu.Instance.EnterLobby ();
	}

	public void Cancel()
	{
		gameObject.SetActive (false);
	}

	public void Plus()
	{

		Debug.Log (LobbyPlayerIdentity.Instance);
		PlayersInRoom++;
	}

	public void Minus()
	{
		PlayersInRoom--;
	}
}
