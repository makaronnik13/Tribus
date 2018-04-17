using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChangeStateEffect :ICardEffect
{
	public bool TryToPlayCard(List<CardEffect> changeStateEffects, List<ISkillAim> aims, Action callback)
	{
		bool result = false;
		foreach(CardEffect changeStateEffect in changeStateEffects)
		{
			if(changeStateEffect.cardAim != CardEffect.CardAim.Cell || changeStateEffect.cellActionType != CardEffect.CellActionType.Evolve)
			{
				continue;
			}
			foreach (ISkillAim aim in aims) {
				if (aim.GetType () == typeof(Block)) {
					NetworkCardGameManager.sInstance.ActivateSkill (aim as Block, changeStateEffect.EvolveType, changeStateEffect.EvolveLevel);
				}
			}
			result = true;
		}
		return result;
	}
}
