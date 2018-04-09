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
					Player aimPlayer = (aims [0] as PlayerVisual).Player;
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

	private void Watch(Player owner, CardEffect effect, List<PlayerVisual> stayedPlayers)
	{
		List<Card> cards = GetCards (owner, effect.NumberOfCards);


		CardsManager.Instance.FillChooseCardField (cards, effect.NumberOfChosenCards, (List<CardVisual> chosenCards)=>{
			Debug.Log(chosenCards.Count);
			BurnCards(owner, chosenCards.Select(c=>c.CardAsset).ToList());
			if(stayedPlayers.Count>0)
			{
				Player aimPlayer = (stayedPlayers[0] as PlayerVisual).Player;
				stayedPlayers.RemoveAt (0);
				Watch (aimPlayer, effect, stayedPlayers);
			}

			if(effect == effects[effects.Count -1 ] && stayedPlayers.Count == 0)
			{
				callback.Invoke();
			}
		});
	}

	private void BurnCards(Player owner, List<Card> chosenCards)
	{
        /*
			List<Card> newPile = owner.Pile.ToList();
			List<Card> newHand = newPile.GetRange(0, owner.CardsInHand);
			newPile =  newPile.GetRange(owner.CardsInHand, newPile.Count - owner.CardsInHand);
			foreach(Card cv in chosenCards)
			{
				owner.CardsInHand--;
				newHand.Remove(cv);
				owner.Drop.Push (cv);
			}
			Queue<Card> pileQueue = new Queue<Card>(newPile);
			foreach(Card c in newHand)
			{
				pileQueue.Enqueue(c);
			}
			owner.Pile = pileQueue;
            */
	}

	private List<Card> GetCards(Player owner, int count)
	{
		List<Card> cards = new List<Card> ();

		cards = owner.Hand;


		cards = cards.OrderBy(x => Guid.NewGuid()).ToList();
		cards = cards.Take (Mathf.Min(cards.Count, count)).ToList();
		return cards;
	}
}


