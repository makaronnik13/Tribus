using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MakeNeutralCardEffect :ICardEffect
{
	public bool TryToPlayCard(List<CardEffect> addCardsEffects, List<ISkillAim> aims, Action callback)
	{
		bool result = false;
		foreach (CardEffect addCardsEffect in addCardsEffects) 
		{
			if(addCardsEffect.cardAim != CardEffect.CardAim.Cell || addCardsEffect.cellActionType!=CardEffect.CellActionType.MakeNeutral)
			{
				continue;
			}
			foreach (ISkillAim aim in aims) {
				if (aim.GetType () == typeof(Block)) {
					NetworkCardGameManager.sInstance.ChangeOwner(aim as Block, null);
				}
			}
			result = true;
		}
		return result;
	}
}
