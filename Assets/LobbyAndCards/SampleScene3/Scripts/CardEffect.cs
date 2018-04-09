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
		Cell,
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

	public enum CellAimType
	{
		Single,
		All,
		Random,
		Circle
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

	public enum CellActionType
	{
		Deevolve,
		MakeOwn,
		MakeNeutral,
		BuffDebuf,
		Evolve,
		DestroyState,
		ChangeBiom
	}

	public enum CellOwnership
	{
		Every,
		Neutral,
		Player,
		Oponent,
		PlayerAndNeutral,
		OponentAndNeutral
	}

	public CardAim cardAim = CardAim.None;

	[ShowIf("AimIsPlayer")]
	public PlayerAimType playerAimType = PlayerAimType.Enemy;
	[ShowIf("AimIsCell")]
	public CellAimType cellAimType = CellAimType.Single;
	[ShowIf("AimIsPlayer")]
	public PlayerActionType playerActionType = PlayerActionType.TakeCards;
	[ShowIf("AimIsCell")]
	public CellActionType cellActionType = CellActionType.Evolve;
	[ShowIf("AimIsCell")]
	public CellOwnership cellOwnership = CellOwnership.Player;
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

	private bool AimIsCell()
	{
		return cardAim == CardAim.Cell;
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

	[ShowIf("ShowCellsFilter")]
	public List<CombineModel.Biom> biomsFilter = new List<CombineModel.Biom>();
	[ShowIf("ShowCellsFilter")]
	public List<CellState> statesFilter = new List<CellState>();
	private bool ShowCellsFilter()
	{
		return (cardAim == CardAim.Cell);
	}

	[ShowIf("Evolve")]
	public CombineModel.Skills EvolveType;
	[ShowIf("Evolve")]
	public int EvolveLevel;
	private bool Evolve()
	{
		if(cardAim == CardAim.Cell)
		{
			if(cellActionType == CellActionType.Evolve)
			{
				return true;
			}
		}
		return false;
	}

	[ShowIf("ShowBiom")]
	public CombineModel.Biom BiomToChange = CombineModel.Biom.None;
	private bool ShowBiom()
	{
		if(cardAim == CardAim.Cell)
		{
			if(cellActionType == CellActionType.ChangeBiom)
			{
				return true;
			}
		}
		return false;
	}

	[ShowIf("ShowNumberOfCells")]
	public int NumberOfCells = 1;
	private bool ShowNumberOfCells()
	{
		if(cardAim == CardAim.Cell && cellAimType == CellAimType.Random)
		{
			return true;
		}
		return false;
	}

	[ShowIf("ShowRadius")]
	public int Radius = 1;
	private bool ShowRadius()
	{
		if(cardAim == CardAim.Cell && cellAimType == CellAimType.Circle)
		{
			return true;
		}
		return false;
	}
}
