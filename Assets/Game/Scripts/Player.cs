using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Player
{
	public string PlayerName;
	public Color PlayerColor;
	public Sprite PlayerAvatar;
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

    private List<Card> hand = new List<Card>();
    public List<Card> Hand
    {
        get
        {
            return hand;
        }
        set
        {
            hand = value;
        }
    }

    public Player(string name, Color color, Sprite avatar, List<Card> deck)
	{
        PlayerName = name;
        PlayerColor = color;
        PlayerAvatar = avatar;
		Pile = new Queue<Card>(deck.OrderBy (a => Guid.NewGuid()));
	}
}
