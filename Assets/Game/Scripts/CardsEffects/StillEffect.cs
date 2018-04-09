using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class StillEffect :ICardEffect
{
	private List<CardEffect> effects = new List<CardEffect> ();
	private Action callback;

	public bool TryToPlayCard(List<CardEffect> observeEffects, List<ISkillAim> aims, Action callback)
	{
		this.callback = callback;
		effects = observeEffects;
		foreach(CardEffect observeEffect in observeEffects)
		{
			if(observeEffect.cardAim!=CardEffect.CardAim.Player || observeEffect.playerActionType!=CardEffect.PlayerActionType.StillCards)
			{
				continue;
			}

			if(aims.Count>0)
			{
				if (observeEffect.NumberOfChosenCards < observeEffect.NumberOfCards && observeEffect.NumberOfChosenCards!=0) {
					Debug.Log ("steel with delay");
					Player aimPlayer = (aims [0] as PlayerVisual).Player;
					List<PlayerVisual> stayedPlayers = new List<PlayerVisual> ();
					foreach (ISkillAim isa in aims) {
						stayedPlayers.Add (isa as PlayerVisual);
					}
					stayedPlayers.RemoveAt (0);
					Watch (aimPlayer, observeEffect, stayedPlayers);
				} else 
				{
					Debug.Log ("steel");
					List<Card> stollenCards = new List<Card> ();
					foreach(ISkillAim p in aims)
					{
						stollenCards.AddRange (GetCards(observeEffect.cardsAimType,((PlayerVisual)p).Player,observeEffect.NumberOfCards));
						BurnCards (((PlayerVisual)p).Player, observeEffect.cardsAimType, GetCards(observeEffect.cardsAimType,((PlayerVisual)p).Player,observeEffect.NumberOfCards));
					}
					AddCards (observeEffect, stollenCards);
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

			AddCards (effect, chosenCards.Select(c=>c.CardAsset).ToList());
		});
	}

	private void BurnCards(Player owner, CardEffect.CardsAimType aim, List<Card> chosenCards)
	{
        /*
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
			List<Card> newHand = newPile.GetRange(0, owner.CardsInHand);
			newPile =  newPile.GetRange(owner.CardsInHand, newPile.Count - owner.CardsInHand);
			foreach(Card cv in chosenCards)
			{
				owner.CardsInHand--;
				newHand.Remove(cv);
			}
			Queue<Card> pileQueue = new Queue<Card>(newPile);
			foreach(Card c in newHand)
			{
				pileQueue.Enqueue(c);
			}
			owner.Pile = pileQueue;
			break;
		case CardEffect.CardsAimType.Pile:
			List<Card> newPile2 = owner.Pile.ToList();
			newPile2 =  newPile2.GetRange(owner.CardsInHand, newPile2.Count - owner.CardsInHand);
			foreach(Card cv in chosenCards)
			{
				newPile2.Remove(cv);
			}
			owner.Pile = new Queue<Card>(newPile2);
			break;
		case CardEffect.CardsAimType.All:

			List<Card> newPlayerDrop = owner.Drop.ToList();
			List<Card> newPlayerHand = owner.Pile.ToList().GetRange(0, owner.CardsInHand);
			List<Card> newPlayerPile = owner.Pile.ToList().GetRange(owner.CardsInHand, owner.Pile.Count - owner.CardsInHand);

			foreach(Card cv in chosenCards)
			{
				if(newPlayerDrop.Contains(cv))
				{
					newPlayerDrop.Remove(cv);
					continue;
				}
				if(newPlayerPile.Contains(cv))
				{
					newPlayerPile.Remove(cv);
					continue;
				}
				if(newPlayerHand.Contains(cv))
				{
					owner.CardsInHand--;
					newPlayerHand.Remove(cv);
					continue;
				}
			}
			owner.Drop = new Stack<Card>(newPlayerDrop);
			owner.Pile = new Queue<Card>(newPlayerPile);
			foreach(Card c in newPlayerHand)
			{
				owner.Pile.Enqueue(c);
			}
			break;
		}
        */
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

	private void AddCards(CardEffect cardAim, List<Card> addedCards)
	{
		switch(cardAim.cardsAimType2)
		{
		case CardEffect.CardsAimType.Hand:
				CardsManager.Instance.AddCardsToPile (addedCards);
				for(int i = 0; i< addedCards.Count;i++)
				{
					CardsManager.Instance.GetCard ();
				}	
			break;
		case CardEffect.CardsAimType.Drop:
				CardsManager.Instance.AddCardsToDrop (addedCards);
			break;
		case CardEffect.CardsAimType.Pile:
				CardsManager.Instance.AddCardsToPile (addedCards, true);
			break;
		}
	}
}


