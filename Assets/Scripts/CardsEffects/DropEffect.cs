using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DropEffect :ICardEffect
{
	private List<CardEffect> effects = new List<CardEffect> ();
	private Action callback;

	public bool TryToPlayCard(List<CardEffect> observeEffects, List<ISkillAim> aims, Action callback)
	{
		this.callback = callback;
		effects = observeEffects;
		foreach(CardEffect observeEffect in observeEffects)
		{
			if(observeEffect.cardAim!=CardEffect.CardAim.Player || observeEffect.playerActionType!=CardEffect.PlayerActionType.DropCards)
			{
				continue;
			}

			if(aims.Count>0)
			{
				if (observeEffect.NumberOfChosenCards < observeEffect.NumberOfCards && observeEffect.NumberOfChosenCards!=0) {
					PhotonPlayer aimPlayer = (aims [0] as PlayerVisual).Player;
					List<PlayerVisual> stayedPlayers = new List<PlayerVisual> ();
					foreach (ISkillAim isa in aims) {
						stayedPlayers.Add (isa as PlayerVisual);
					}
					stayedPlayers.RemoveAt (0);
					Watch (aimPlayer, observeEffect, stayedPlayers);
				} else 
				{
					foreach(ISkillAim p in aims)
					{
						BurnCards (((PlayerVisual)p).Player, GetCards(((PlayerVisual)p).Player,observeEffect.NumberOfCards));
					}
					return true;
				}
			}
		}
		return false;
	}

	private void Watch(PhotonPlayer owner, CardEffect effect, List<PlayerVisual> stayedPlayers)
	{
		List<Card> cards = GetCards (owner, effect.NumberOfCards);
		CardsManager.Instance.ChooseManager.FillChooseCardField (cards, effect.NumberOfChosenCards, (List<CardVisual> chosenCards)=>{

            BurnCards(owner, chosenCards.Select(c=>c.CardAsset).ToList());
			if(stayedPlayers.Count>0)
			{
				PhotonPlayer aimPlayer = (stayedPlayers[0] as PlayerVisual).Player;
				stayedPlayers.RemoveAt (0);
				Watch (aimPlayer, effect, stayedPlayers);
			}

			if(effect == effects[effects.Count -1 ] && stayedPlayers.Count == 0)
			{
				callback.Invoke();
			}
				
			foreach(CardVisual pv in chosenCards)
			{
				GameObject.Destroy(pv.gameObject);
			}
		});
	}

	private void BurnCards(PhotonPlayer owner, List<Card> chosenCards)
	{
		NetworkCardGameManager.sInstance.DropCards (owner, chosenCards);
	}

	private List<Card> GetCards(PhotonPlayer owner, int count)
	{
		List<Card> cards = new List<Card> ();
		cards = NetworkCardGameManager.sInstance.GetPlayerHand(owner);
		cards = cards.OrderBy(x => Guid.NewGuid()).ToList();
		cards = cards.Take (Mathf.Min(cards.Count, count)).ToList();
		return cards;
	}
}


