using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AllCardsView : MonoBehaviour {

	public GameObject CardPrefab;
	public Transform Hab;

	private List<LibraryCardPanel> cardsPanels = new List<LibraryCardPanel>();

	public void AddCard(Card card)
	{
		LibraryCardPanel cp = null;

		foreach(LibraryCardPanel lcp in cardsPanels)
		{
			if(lcp.Card == card)
			{
				cp = lcp;
			}
		}


		if (!cp) {
			cp = Instantiate (CardPrefab).GetComponent<LibraryCardPanel> ();
			cp.Init (card);
			cp.transform.SetParent (Hab);
			cp.transform.localScale = Vector3.one;
			cardsPanels.Add (cp);
		} else {
			cp.Add ();
		}
	}

	public void RemoveCard(Card card)
	{
		LibraryCardPanel cp = null;
		foreach(LibraryCardPanel lcp in cardsPanels)
		{
			if(lcp.Card == card)
			{
				cp = lcp;
			}
		}

		if (cp)
		{
			cp.Remove ();
		}	
	}

	public void Init(DeckStruct ds)
	{
		foreach(LibraryCardPanel lcp in cardsPanels)
		{
			Destroy (lcp.gameObject);
		}
		cardsPanels.Clear ();

		foreach(Card card in LobbyPlayerIdentity.Instance.player.AllCards)
		{
			AddCard (card);
		}

		foreach(Card c in ds.Cards)
		{
			RemoveCard (c);
		}
	}
}
