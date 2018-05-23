using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : Singleton<PlayerStats> {

    public int CurrentPartyLevel = 0;

	public Action<int> OnMoneyChanged = (int v)=>{};

	private int playerMoney = 50;
	public int PlayerMoney
	{
		get
		{
			return playerMoney;
		}
		set
		{
			playerMoney = value;
			OnMoneyChanged (playerMoney);
		}
	}

	private List<Card> playerCards = new List<Card>();
	public List<Card> PlayerCards
	{
		get
		{
			return playerCards;
		}
		set
		{
			playerCards = value;
		}
	}

	private List<Item> playerItems = new List<Item>();
	public List<Item> PlayerItems
	{
		get
		{
			return playerItems;
		}
		set
		{
			playerItems = value;
		}
	}
		

	void Start()
	{
		PlayerMoney = 50;
	}

}
