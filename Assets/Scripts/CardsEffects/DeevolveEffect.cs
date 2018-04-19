using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DeevolveEffect :ICardEffect
{
	public bool TryToPlayCard(List<CardEffect> addCardsEffects, List<ISkillAim> aims, Action callback)
	{
		bool result = false;
		foreach(CardEffect addCardsEffect in addCardsEffects)
		{
			if(addCardsEffect.cardAim != CardEffect.CardAim.Cell || addCardsEffect.cellActionType!=CardEffect.CellActionType.Deevolve)
			{
				continue;
			}
		foreach (ISkillAim aim in aims) 
		{
			if (aim.GetType () == typeof(Block)) 
			{						
				List<CellState> previousStates = DefaultResourcesManager.PreviousStates ((aim as Block).State);

				if(previousStates.Count>0)
				{
					CellState cs = previousStates[UnityEngine.Random.Range(0, previousStates.Count()-1)];
                    NetworkCardGameManager.sInstance.ChangeState(aim as Block, cs);
				}
			}
		}
			result = true;
		}
		return result;
	}
}
