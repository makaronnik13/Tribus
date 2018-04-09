using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecsPanel : MonoBehaviour {

	public Transform Dock;
	public GameObject AddDeckButton;
	public GameObject DeckPrefab;

	void OnEnable()
	{
		ClearDock ();
		foreach(DeckStruct ds in LobbyPlayerIdentity.Instance.player.Decks)
		{
			AddDeckToDock (ds);
		}
			
		AddDeckButton.SetActive (Dock.childCount != 9);
	}
		

	private void ClearDock()
	{
		foreach(Transform t in Dock)
		{
			if(t.gameObject!= AddDeckButton)
			{
				Destroy (t.gameObject);
			}
		}
	}

	private void AddDeckToDock(DeckStruct ds)
	{
		Transform deck = Instantiate (DeckPrefab).transform;
		deck.transform.SetParent (Dock);
		deck.transform.localScale = Vector3.one;
		deck.SetAsFirstSibling ();
		deck.GetComponent<DeckButton> ().Init (ds);
		AddDeckButton.SetActive (Dock.childCount != 9);
	}

	public void CreateDeck()
	{
		DeckStruct newDs = new DeckStruct ();
		newDs.DeckName = "NewDeck";
		LobbyPlayerIdentity.Instance.player.Decks.Add (newDs);
		Transform deck = Instantiate (DeckPrefab).transform;
		deck.transform.SetParent (Dock);
		deck.transform.localScale = Vector3.one;
		deck.GetComponent<DeckButton> ().Init (newDs);
		deck.SetAsFirstSibling ();
		deck.SetSiblingIndex(deck.GetSiblingIndex () - 1);
		AddDeckButton.SetActive (Dock.childCount != 9);
		LobbyMenu.Instance.EditDeck (newDs);
	}
}
