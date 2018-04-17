using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
public class PlayerSaveStruct
{
    [NonSerialized]
	public Action<PlayerSaveStruct> OnParamsChanged = (PlayerSaveStruct p)=>{};

    [NonSerialized]
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
    private int playerAvatarId;
	public int PlayerAvatarId
	{
		get
		{
			return playerAvatarId;
		}
		set
		{
			playerAvatarId = value;
			OnParamsChanged.Invoke (this);
		}
	}


    [SerializeField]
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
            List<DeckStruct> newDecks = new List<DeckStruct>();
            foreach (DeckStruct ds in value)
            {
                DeckStruct newDeck = new DeckStruct
                {
                    DeckName = ds.DeckName,
                    Cards = ds.Cards
                };

                newDecks.Add(newDeck);
            }

            decks.Clear();
            decks = newDecks;

            OnDecksChanged.Invoke (this);
		}
	}

   
	[SerializeField]
	public List<int> AllCards = new List<int>();
	
}
