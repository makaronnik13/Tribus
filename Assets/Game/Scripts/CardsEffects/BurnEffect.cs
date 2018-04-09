using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BurnEffect :ICardEffect
{
	private List<CardEffect> effects = new List<CardEffect> ();
	private Action callback;

	public bool TryToPlayCard(List<CardEffect> observeEffects, List<ISkillAim> aims, Action callback)
	{
		this.callback = callback;
		effects = observeEffects;
		foreach(CardEffect observeEffect in observeEffects)
		{
			if(observeEffect.cardAim!=CardEffect.CardAim.Player || observeEffect.playerActionType!=CardEffect.PlayerActionType.BurnCards)
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
						BurnCards (((PlayerVisual)p).Player, observeEffect.cardsAimType, GetCards(observeEffect.cardsAimType,((PlayerVisual)p).Player,observeEffect.NumberOfCards));
					}
					return true;
				}
			}
		}
		return false;
	}

	private void Watch(Player owner, CardEffect effect, List<PlayerVisual> stayedPlayers)
	{

		CardEffect.CardsAimType aim = effect.cardsAimType;
		List<Card> cards = GetCards (effect.cardsAimType, owner, effect.NumberOfCards);


		CardsManager.Instance.FillChooseCardField (cards, effect.NumberOfChosenCards, (List<CardVisual> chosenCards)=>{
			Debug.Log(chosenCards.Count);
			BurnCards(owner, aim, chosenCards.Select(c=>c.CardAsset).ToList());
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

	private void BurnCards(Player owner, CardEffect.CardsAimType aim, List<Card> chosenCards)
	{
		Debug.Log (chosenCards.Count);
		switch(aim)
		{
		case CardEffect.CardsAimType.Drop:
			List<Card> newDrop =	owner.Drop.ToList();
			foreach(Card cv in chosenCards)
			{
				newDrop.Remove(cv);
			}
			owner.Drop = new Stack<Card>(newDrop);
			break;
		case CardEffect.CardsAimType.Hand:
			List<Card> newPile = owner.Pile.ToList();
			//List<Card> newHand = newPile.GetRange(0, owner.CardsInHand);
			//newPile =  newPile.GetRange(owner.CardsInHand, newPile.Count - owner.CardsInHand);
			foreach(Card cv in chosenCards)
			{
				//owner.CardsInHand--;
				//newHand.Remove(cv);
			}
			Queue<Card> pileQueue = new Queue<Card>(newPile);
			//foreach(Card c in newHand)
			//{
			//	pileQueue.Enqueue(c);
			//}
			owner.Pile = pileQueue;
			break;
		case CardEffect.CardsAimType.Pile:
			List<Card> newPile2 = owner.Pile.ToList();
			//newPile2 =  newPile2.GetRange(owner.CardsInHand, newPile2.Count - owner.CardsInHand);
			foreach(Card cv in chosenCards)
			{
				//newPile2.Remove(cv);
			}
			owner.Pile = new Queue<Card>(newPile2);
			break;
		case CardEffect.CardsAimType.All:

			List<Card> newPlayerDrop = owner.Drop.ToList();
			//List<Card> newPlayerHand = owner.Pile.ToList().GetRange(0, owner.CardsInHand);
			//List<Card> newPlayerPile = owner.Pile.ToList().GetRange(owner.CardsInHand, owner.Pile.Count - owner.CardsInHand);

			foreach(Card cv in chosenCards)
			{
				if(newPlayerDrop.Contains(cv))
				{
					newPlayerDrop.Remove(cv);
					continue;
				}
				/*if(newPlayerPile.Contains(cv))
				{
					newPlayerPile.Remove(cv);
					continue;
				}
				if(newPlayerHand.Contains(cv))
				{
					owner.CardsInHand--;
					newPlayerHand.Remove(cv);
					continue;
				}*/
			}
			owner.Drop = new Stack<Card>(newPlayerDrop);
			//owner.Pile = new Queue<Card>(newPlayerPile);
			//foreach(Card c in newPlayerHand)
			//{
			//	owner.Pile.Enqueue(c);
			//}
			break;
		}

	}

	private List<Card> GetCards(CardEffect.CardsAimType aim, Player owner, int count)
	{
		List<Card> cards = new List<Card> ();
		switch(aim)
		{
		case CardEffect.CardsAimType.Drop:
			cards = owner.Drop.ToList();
			break;
		case CardEffect.CardsAimType.Hand:
			//cards = owner.Pile.ToList().GetRange(0, owner.CardsInHand);
			break;
		case CardEffect.CardsAimType.Pile:
			//cards = owner.Pile.ToList ().GetRange (owner.CardsInHand, owner.Pile.Count - owner.CardsInHand);
			break;
		case CardEffect.CardsAimType.All:
			cards = owner.Pile.Concat (owner.Drop).ToList();
			break;
		}

		cards = cards.OrderBy(x => Guid.NewGuid()).ToList();
		cards = cards.Take (Mathf.Min(cards.Count, count)).ToList();
		return cards;
	}
}

