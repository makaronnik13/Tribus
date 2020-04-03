using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CardPlayCondition : MonoBehaviour 
{

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
		
	public AimCondition aimConditionType;

	public CardsAim cardsAim;
	[ShowIf("ShowCardsList")]
	public List<Card> checkingCards;
	public CardsTypes cardsTypes;
	[ShowIf("ShowCards")]
	public ConditionCheckType conditionCheckType;
	public int count;




	public bool Check(ISkillAim aim = null)
	{
		
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
