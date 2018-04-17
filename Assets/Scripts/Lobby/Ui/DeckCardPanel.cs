using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DeckCardPanel : MonoBehaviour 
{
	public TextMeshProUGUI Counter, CardName;
	public Image CardImage;
	private int count = 1;
	private int Count
	{
		get
		{
			return count;
		}
		set
		{
			count = value;
			Counter.text = "" + count;
			Counter.enabled = (count!=1);
		}
	}
	private Card card;
	public Card Card
	{
		get
		{
			return card;
		}
	}

	public void Init(Card c)
	{
		card = c;
		Count = 1;
		CardName.text = c.CardName;
		CardImage.sprite = c.cardSprite;
	}

	public void Add()
	{
		Count++;
	}

	public void Remove(Action destroyCallback)
	{
		Count--;
		if(Count == 0)
		{
			destroyCallback ();
		}
	}

	public void Push()
	{
		GetComponentInParent<DeckEditPanel> ().TapCard (this);
	}
}
