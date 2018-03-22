using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


[System.Serializable]
public class Player
{
	public string name;
	public Color color;
	public Sprite avatar;

	public Deck deck; 

	private Queue<Card> pile = new Queue<Card> ();
	public Queue<Card> Pile { 
		get
		{
			return pile;
		}
		set
		{
			pile = value;
		}
	}

	private Stack<Card> drop = new Stack<Card> ();
	public Stack<Card> Drop { 
		get
		{
			return drop;
		}
		set
		{
			drop = value;
		}
	}

	public int CardsInHand {get;set;}

	public void InitPlayer()
	{
		Pile = new Queue<Card>(deck.cards.OrderBy (a => Guid.NewGuid()));
	}
}
