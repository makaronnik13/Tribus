using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LibraryCardPanel : MonoBehaviour {

	public GameObject Counter;

	private CardView _cardView;
	private CardView cardView
	{
		get
		{
			if(!_cardView)
			{
				_cardView = GetComponentInChildren<CardView> ();
			}
			return _cardView;
		}
	}

	private int count;
	public int Count
	{
		get
		{
			return count;
		}
		set
		{
			count = value;
			Counter.GetComponentInChildren<TextMeshProUGUI> ().text = "" + count;
			GetComponent<Button> ().interactable = (count != 0);
			Counter.SetActive (count > 0);
		}
	}

	private Card _card;
	public Card Card
	{
		get
		{
			return _card;//	cardView.CardAsset;
		}
	}

	public void Add()
	{
		Count++;
	}

	public void Remove()
	{
		Count--;
	}

	public void Init(Card c)
	{
		_card = c;
		cardView.Init (c);
		Count = 1;
	}

	public void Push()
	{
		GetComponentInParent<DeckEditPanel> ().TapCard (this);
	}
}

