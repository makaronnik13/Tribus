using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChangeBiomEffect:ICardEffect
{
	public bool TryToPlayCard(List<CardEffect> changeBiomEffects, List<ISkillAim> aims, Action callback)
	{ 
		bool result = false;
		foreach (CardEffect changeBiomEffect in changeBiomEffects) 
		{
			if(changeBiomEffect.cardAim != CardEffect.CardAim.Cell || changeBiomEffect.cellActionType!=CardEffect.CellActionType.ChangeBiom)
			{
				continue;
			}
			foreach (ISkillAim aim in aims) {
				if (aim.GetType () == typeof(Block)) {
					(aim as Block).Biom = changeBiomEffect.BiomToChange;
				}
			}
			result = true;
		}
		return result;
	}
}