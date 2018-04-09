using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerIdentity : Singleton<LobbyPlayerIdentity> 
{
	public PlayerSaveStruct player;

	private void Start()
	{
		InitPlayerDefault ();
		DontDestroyOnLoad (this);
	}

	private void InitPlayerDefault()
	{
		player.PlayerName = DefaultResourcesManager.GetRandomName();
		player.PlayerColor = DefaultResourcesManager.GetRandomColor();
		player.PlayerAvatar = DefaultResourcesManager.GetRandomAvatar ();
		player.Decks = new List<DeckStruct> (){ DefaultResourcesManager.StartingDeck};

		for (int i = 0; i < 3; i++) 
		{
			player.AllCards.AddRange (DefaultResourcesManager.AllCards);
		}
	}
}
