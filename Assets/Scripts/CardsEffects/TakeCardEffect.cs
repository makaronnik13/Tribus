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
            
			if(effect.cardAim!=CardEffect.CardAim.Player || effect.playerActionType!=CardEffect.PlayerActionType.TakeCards)
			{
				continue;
			}
				
			foreach (ISkillAim aim in aims) 
			{
				if (aim.GetType () == typeof(PlayerVisual)) 
				{
					PhotonPlayer player = (aim as PlayerVisual).Player;
					for(int i = 0; i<effect.NumberOfCards;i++)
					{
						NetworkCardGameManager.sInstance.PlayerGetCard(player);
					}
				}
			}
			result = true;
            
		}
        
		return result;
	}
}
