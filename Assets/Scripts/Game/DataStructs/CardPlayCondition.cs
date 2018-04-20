using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CardPlayCondition : MonoBehaviour 
{
	public enum ConditionType
	{
		Cell,
		State,
		Biom,
		Cards
	}

	public enum AimCondition
	{
		Player,
		AnyPlayer,
		AnyEnemy,
		AimPlayer,
		AllPlayers
	}

	public enum CardsAim
	{
		All,
		Hand,
		Pile,
		Drop
	}

	public enum CardsTypes
	{
		Any,
		Custom
	}

	public enum ConditionCheckType
	{
		Equeal,
		More,
		Less,
		MoreOrEq,
		LessOrEq
	}

	public ConditionType conditionType;
	public AimCondition aimConditionType;

	[ShowIf("ShowBioms")]
	public List<CombineModel.Biom> checkingBioms;
	[ShowIf("ShowCardAim")]
	public CardsAim cardsAim;
	[ShowIf("ShowStates")]
	public List<CellState> checkingStates;
	[ShowIf("ShowCardsList")]
	public List<Card> checkingCards;
	[ShowIf("ShowCards")]
	public CardsTypes cardsTypes;
	[ShowIf("ShowCards")]
	public ConditionCheckType conditionCheckType;
	public int count;


	private bool ShowBioms()
	{
		return conditionType == ConditionType.Biom;
	}

	private bool ShowStates()
	{
		return conditionType == ConditionType.State;
	}

	private bool ShowCards()
	{
		return conditionType == ConditionType.Cards;
	}

	private bool ShowCardsList()
	{
		return conditionType == ConditionType.Cards && cardsTypes == CardsTypes.Custom;
	}

	private bool ShowCardAim()
	{
		return conditionType == ConditionType.Cards;
	}

	public bool Check(ISkillAim aim = null)
	{
		switch(conditionType)
		{
			case ConditionType.Biom:
				break;
			case ConditionType.Cell:
				switch(aimConditionType)
				{
					case AimCondition.AimPlayer:
						break;
					case AimCondition.AnyEnemy:
						break;
					case AimCondition.Player:
						break;
					case AimCondition.AnyPlayer:
						break;
					case AimCondition.AllPlayers:
						break;
				}
				break;
			case ConditionType.State:
				break;
			case ConditionType.Cards:
			/*switch(aimConditionType)
				{
					case AimCondition.AimPlayer:
						break;
					case AimCondition.AnyEnemy:
						break;
					case AimCondition.Player:
						break;
					case AimCondition.AnyEnemy:
						break;
					case AimCondition.AllPlayers:
						break;
				}
				*/
				break;
		}

		return false;
	}

	private bool CheckConditionNumber(int number)
	{
		switch (conditionCheckType)
		{
		case ConditionCheckType.Equeal:
			return number == count;
		case ConditionCheckType.Less:
			return number < count;
		case ConditionCheckType.LessOrEq:
			return number <= count;
		case ConditionCheckType.More:
			return number > count;
		case ConditionCheckType.MoreOrEq:
			return number >= count;
		}

		return false;
	}
}
