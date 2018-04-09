using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TakeCardEffect :ICardEffect
{

	public bool TryToPlayCard(List<CardEffect> effects, List<ISkillAim> aims, Action callback)
	{
		bool result = false;
		foreach (CardEffect effect in effects) 
		{
            /*
			if(effect.cardAim!=CardEffect.CardAim.Player || effect.playerActionType!=CardEffect.PlayerActionType.TakeCards)
			{
				continue;
			}
				
			foreach (ISkillAim aim in aims) 
			{
				if (aim.GetType () == typeof(PlayerVisual)) 
				{						
					if ((aim as PlayerVisual).Player == GameLobby.Instance.CurrentPlayer) 
					{
							for (int i = 0; i < effect.NumberOfCards; i++) {
								CardsManager.Instance.GetCard ();
							}	
					} else {
						(aim as PlayerVisual).Player.CardsInHand += effect.NumberOfCards;
					}
				}
			}
			result = true;
            */
		}
        
		return result;
	}
}
