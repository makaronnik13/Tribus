using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DestroyStateEffect :ICardEffect
{
	public bool TryToPlayCard(List<CardEffect> addCardsEffects, List<ISkillAim> aims, Action callback)
	{
		bool result = false;

		foreach (CardEffect addCardsEffect in addCardsEffects) 
		{
			if(addCardsEffect.cardAim != CardEffect.CardAim.Cell || addCardsEffect.cellActionType!=CardEffect.CellActionType.DestroyState)
			{
				continue;
			}
			foreach (ISkillAim aim in aims) {
				if (aim.GetType () == typeof(Block)) {
					if (!BlocksField.Instance.baseStates.Contains ((aim as Block).State))
                    {
                        CellState state = null;

                        foreach (CellState cs in BlocksField.Instance.baseStates)
                        {
                            if (cs && cs.Biom == (aim as Block).Biom)
                            {
                                state = cs;
                            }
                        }
                        NetworkCardGameManager.sInstance.ChangeState(aim as Block, state);
					}
				}
			}
			result = true;
		}
		return result;
	}
}
