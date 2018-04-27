using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]

public class CardEffect 
{
	public enum CardAim
	{
		None,
		Player
	}

	public enum PlayerAimType
	{
		You,
		Any,
		All,
		Enemy,
		Enemies
	}

	public enum NoneType
	{
		Win,
		Choose
	}
		

	public enum CardsAimType
	{
		Pile,
		Hand,
		Drop,
		All
	}

	public enum PlayerActionType
	{
		AddCards,
		DropCards,
		BurnCards,
		WatchCards,
		TakeCards,
		StillCards
	}


	public CardAim cardAim = CardAim.None;

	[ShowIf("AimIsPlayer")]
	public PlayerAimType playerAimType = PlayerAimType.Enemy;
	[ShowIf("AimIsPlayer")]
	public PlayerActionType playerActionType = PlayerActionType.TakeCards;
	[ShowIf("AimIsCards")]
	public CardsAimType cardsAimType = CardsAimType.Hand;
	[ShowIf("AimIsCards2")]
	public CardsAimType cardsAimType2 = CardsAimType.Hand;
	[ShowIf("NoAim")]
	public NoneType noneType = NoneType.Choose;

	private bool NoAim()
	{
		return cardAim == CardAim.None;
	}

	private bool AimIsPlayer()
	{
		return cardAim == CardAim.Player;
	}
		

	private bool AimIsCards()
	{
		return (cardAim == CardAim.Player && (playerActionType == PlayerActionType.StillCards || playerActionType == PlayerActionType.BurnCards || playerActionType == PlayerActionType.AddCards|| playerActionType == PlayerActionType.WatchCards));
	}

	private bool AimIsCards2()
	{
		return (cardAim == CardAim.Player && playerActionType == PlayerActionType.StillCards);
	}

	[ShowIf("ShowCardsList")]
	public List<Card> Cards = new List<Card>();

	private bool ShowCardsList()
	{
		if(cardAim == CardAim.Player)
		{
			if(playerActionType == PlayerActionType.AddCards)
			{
				return true;
			}
		}

		if (cardAim == CardAim.None && noneType == NoneType.Choose) 
		{
			return true;
		}

		return false;
	}

	[ShowIf("ShowNumberOfCards")]
	public int NumberOfCards = 1;
	private bool ShowNumberOfCards()
	{
		if(cardAim == CardAim.Player)
		{
			if(playerActionType == PlayerActionType.BurnCards || playerActionType == PlayerActionType.DropCards || playerActionType == PlayerActionType.TakeCards || playerActionType == PlayerActionType.WatchCards || playerActionType == PlayerActionType.StillCards)
			{
				return true;
			}
		}

		return false;
	}

	[ShowIf("ShowNumberOfChosenCards")]
	public int NumberOfChosenCards = 0;
	private bool ShowNumberOfChosenCards()
	{
		if(cardAim == CardAim.Player)
		{
			if(playerActionType == PlayerActionType.BurnCards || playerActionType == PlayerActionType.DropCards || playerActionType == PlayerActionType.AddCards || playerActionType == PlayerActionType.StillCards)
			{
				return true;
			}
		}

		return false;
	}

}
