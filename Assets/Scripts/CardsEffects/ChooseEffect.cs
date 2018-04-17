using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ChooseEffect :ICardEffect
{
	private Action callback;
	public bool TryToPlayCard(List<CardEffect> observeEffects, List<ISkillAim> aims, Action callback)
	{
		this.callback = callback;
		foreach(CardEffect observeEffect in observeEffects)
		{
			if(observeEffect.cardAim!=CardEffect.CardAim.None || observeEffect.noneType != CardEffect.NoneType.Choose)
			{
				continue;
			}
				
			CardsManager.Instance.FillChooseCardField (observeEffect.Cards, 1, (List<CardVisual> chosenCards)=>
			{
					
			}, false);
		}

		return false;
	}
		
}
