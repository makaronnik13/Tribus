using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int CurrentPartyLevel = 0;

	private List<Item> playerItems = new List<Item>();
	public List<Item> PlayerItems
	{
		get
		{
			return playerItems;
		}
	}

	public void GetItem(Item item)
	{
		//playerItems.Add (playerItems);
	}


}
