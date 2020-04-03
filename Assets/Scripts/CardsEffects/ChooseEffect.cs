using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ChooseEffect :ICardEffect
{
	public bool TryToPlayCard(List<CardEffect> observeEffects, List<ISkillAim> aims, Action callback)
	{
		foreach(CardEffect observeEffect in observeEffects)
		{
			if(observeEffect.cardAim!=CardEffect.CardAim.None)
			{
				continue;
			}
				
			CardsManager.Instance.ChooseManager.FillChooseCardField (observeEffect.Cards, 1, (List<CardVisual> chosenCards)=>
			{
                chosenCards[0].SetState(CardVisual.CardState.ChosingAim);
                CardsPlayer.Instance.PlayCard(chosenCards[0]);    
			});
		}

		return false;
	}
		
}
