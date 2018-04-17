using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DeckStruct
{
    [SerializeField]
	public string DeckName;

    [HideInInspector]
    [SerializeField]
	private List<int> cardsIds = new List<int>();

    [ShowInInspector]
    private List<Card> cards = new List<Card>();

    public List<Card> Cards
    {
        get
        {
            if (cards.Count == 0)
            {
                foreach (int cId in cardsIds)
                {
                    cards.Add(DefaultResourcesManager.AllCards[cId]);
                }
            }
            return cards;
        }
        set
        {
            cards = value;
            cardsIds.Clear();
            foreach (Card c in value)
            {
                cardsIds.Add(DefaultResourcesManager.AllCards.ToList().IndexOf(c));
            }
        }
    }

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
