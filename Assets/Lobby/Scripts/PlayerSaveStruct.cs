using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerSaveStruct
{
	public Action<PlayerSaveStruct> OnParamsChanged = (PlayerSaveStruct p)=>{};
	public Action<PlayerSaveStruct> OnDecksChanged = (PlayerSaveStruct p)=>{};

	[SerializeField]
	private string playerName;
	public string PlayerName
	{
		get
		{
			return playerName;
		}
		set
		{
			playerName = value;
			OnParamsChanged.Invoke (this);
		}
	}

	[SerializeField]
	private Color playerColor;
	public Color PlayerColor
	{
		get
		{
			return playerColor;
		}
		set
		{
			playerColor = value;
			OnParamsChanged.Invoke (this);
		}
	}

	[SerializeField]
	private Sprite playerAvatar;
	public Sprite PlayerAvatar
	{
		get
		{
			return playerAvatar;
		}
		set
		{
			playerAvatar = value;
			OnParamsChanged.Invoke (this);
		}
	}

    public DeckStruct CurrentDeck;

	[SerializeField]
	private List<DeckStruct> decks = new List<DeckStruct>();
	public List<DeckStruct> Decks
	{
		get
		{
			return decks;
		}
		set
		{
			decks = value;
			OnDecksChanged.Invoke (this);
		}
	}

	[SerializeField]
	private List<Card> allCards = new List<Card>();
	public List<Card> AllCards
	{
		get
		{
			return allCards;
		}
		set
		{
			allCards = value;
		}
	}
}
