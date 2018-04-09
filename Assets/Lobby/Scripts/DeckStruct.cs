using System.Collections;
using System.Collections.Generic;
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
}
