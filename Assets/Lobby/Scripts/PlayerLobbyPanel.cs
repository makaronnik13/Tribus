using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class PlayerLobbyPanel : MonoBehaviour {

	public TextMeshProUGUI PlayerName;
	public TMP_Dropdown Decks;
	public Image Avatar, ColorCircle;

	void Awake()
	{
		//LobbyPlayerIdentity.Instance.player.OnDecksChanged += UpdateDeck;
		//LobbyPlayerIdentity.Instance.player.OnParamsChanged += UpdateInfo;
	}

	private void UpdateDeck(PlayerSaveStruct player)
	{
		Decks.ClearOptions ();
		Decks.AddOptions (player.Decks.Where(d=>d.Awaliable).Select(d=>d.DeckName).ToList());
	}

	private void UpdateInfo(PlayerSaveStruct player)
	{
		PlayerName.text = player.PlayerName;
		Avatar.sprite = player.PlayerAvatar;
		ColorCircle.color = player.PlayerColor;
	}

	public void Click()
	{
		//LobbyMenu.Instance.PlayerScreen ();
	}
}
