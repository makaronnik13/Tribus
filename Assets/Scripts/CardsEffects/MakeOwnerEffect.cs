﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MakeOwnerEffect : ICardEffect
{

	public bool TryToPlayCard(List<CardEffect> cardEffects, List<ISkillAim> aims, Action callback)
	{
		bool result = false;

		foreach (CardEffect cardEffect in cardEffects) {
			if(cardEffect.cardAim != CardEffect.CardAim.Cell || cardEffect.cellActionType!=CardEffect.CellActionType.MakeOwn)
			{
				continue;
			}
			foreach (ISkillAim aim in aims) {
				if (aim.GetType () == typeof(Block)) {						
					(aim as Block).Owner = NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer;
				}
			}
			result = true;
		}

		return result;
	}
}