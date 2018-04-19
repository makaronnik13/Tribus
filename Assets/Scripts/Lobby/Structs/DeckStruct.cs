using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DeckStruct
{

    public DeckStruct(string name, List<string> cards)
    {
        DeckName = name;
        Cards = cards;
    }

	public string DeckName;


	public List<string> Cards = new List<string>();
    

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
		foreach(string c in Cards)
		{
            Card card = DefaultResourcesManager.GetCardById(c);
			if(i == 3)
			{
				break;
			}

			if(card.WinCard)
			{
				winSprites [i] = card.cardSprite;
				i++;
			}
		}
		return winSprites;
	}
}
