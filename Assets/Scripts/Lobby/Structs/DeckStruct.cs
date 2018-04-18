using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DeckStruct
{
	public string DeckName;
	public List<Card> Cards = new List<Card>();

	public bool Awaliable
	{
		get
		{
			//cards count and win cards count
			return true;
		}
	}

	public Sprite[] GetWinCardsSprires()
	{
		Sprite[] winSprites = new Sprite[3]{null, null, null};
		int i = 0;
		foreach(Card c in Cards)
		{
			if(i == 3)
			{
				break;
			}

			if(c.WinCard)
			{
				winSprites [i] = c.cardSprite;
				i++;
			}
		}
		return winSprites;
	}

	public void Reinit()
	{
		List<string> ids = new List<string> ();
		foreach(Card c in Cards)
		{
			ids.Add (c.name);
		}
		Cards.Clear ();
		foreach(string id in ids)
		{
			Cards.Add (DefaultResourcesManager.GetCardById(id));
		}
	}
}
