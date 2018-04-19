using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckEditPanel : Singleton<DeckEditPanel> 
{
	private EditingDeckView deckView;
	private AllCardsView cardsView;
	private DeckStruct editingDeck;

	void Awake()
	{
		deckView = GetComponentInChildren<EditingDeckView> ();
		cardsView = GetComponentInChildren<AllCardsView> ();
	}

	public void Edit(DeckStruct ds)
	{
		editingDeck = ds;
		deckView.Init (ds);
		cardsView.Init (ds);
	}

	public void TapCard(LibraryCardPanel lcp)
	{
		editingDeck.Cards.Add (lcp.Card.name);
		deckView.AddCard (lcp.Card.name);
		cardsView.RemoveCard (lcp.Card.name);
		SaveDeck ();
	}

	public void TapCard(DeckCardPanel dcp)
	{
		editingDeck.Cards.Remove (dcp.Card.name);
		deckView.RemoveCard (dcp.Card);
		cardsView.AddCard (dcp.Card);
		SaveDeck ();
	}

	public void SaveDeck()
	{
		if(LobbyPlayerIdentity.Instance)
		{
			LobbyPlayerIdentity.Instance.player.Decks = LobbyPlayerIdentity.Instance.player.Decks;
		}
	}

	public void Apply()
	{
		LobbyMenu.Instance.BackToPlayerSetings ();
	}

	public void DeleteDeck()
	{
		LobbyPlayerIdentity.Instance.player.Decks.Remove (editingDeck);
		Apply ();
	}
}
