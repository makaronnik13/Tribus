﻿using System.Collections;
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
						BurnCards (((PlayerVisual)p).Player, observeEffect.cardsAimType, GetCards(observeEffect.cardsAimType,((PlayerVisual)p).Player,observeEffect.NumberOfCards));
					}
					return true;
				}
			}
		}
		return false;
	}

	private void Watch(PhotonPlayer owner, CardEffect effect, List<PlayerVisual> stayedPlayers)
	{

		CardEffect.CardsAimType aim = effect.cardsAimType;
		List<Card> cards = GetCards (effect.cardsAimType, owner, effect.NumberOfCards);


		CardsManager.Instance.FillChooseCardField (cards, effect.NumberOfChosenCards, (List<CardVisual> chosenCards)=>{
			Debug.Log(chosenCards.Count);
			BurnCards(owner, aim, chosenCards.Select(c=>c.CardAsset).ToList());
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
		});
	}

	private void BurnCards(PhotonPlayer owner, CardEffect.CardsAimType aim, List<Card> chosenCards)
	{
		Debug.Log (chosenCards.Count);
		switch(aim)
		{
		case CardEffect.CardsAimType.Drop:
                foreach (Card c in chosenCards)
                {
                    NetworkCardGameManager.sInstance.RemoveCardFromDrop(c, owner);
                }
                break;
		case CardEffect.CardsAimType.Hand:
                foreach (Card c in chosenCards)
                {
                    NetworkCardGameManager.sInstance.RemoveCardFromHand(c, owner);
                }
                break;
		case CardEffect.CardsAimType.Pile:
                foreach (Card c in chosenCards)
                {
                    NetworkCardGameManager.sInstance.RemoveCardFromPile(c, owner);
                }
                break;
		case CardEffect.CardsAimType.All:

                foreach (Card c in chosenCards)
                {
                    NetworkCardGameManager.sInstance.RemoveCardFromPlayer(c, owner);
                }
                break;
		}

	}

	private List<Card> GetCards(CardEffect.CardsAimType aim, PhotonPlayer owner, int count)
	{
		List<Card> cards = new List<Card> ();
		switch(aim)
		{
		case CardEffect.CardsAimType.Drop:
            cards = NetworkCardGameManager.sInstance.GetPlayerDrop(owner);
			break;
		case CardEffect.CardsAimType.Hand:
                cards = NetworkCardGameManager.sInstance.GetPlayerHand(owner);
                break;
		case CardEffect.CardsAimType.Pile:
                cards = NetworkCardGameManager.sInstance.GetPlayerPile(owner);
                break;
		case CardEffect.CardsAimType.All:
                cards = NetworkCardGameManager.sInstance.GetPlayerCards(owner);
                break;
		}

		cards = cards.OrderBy(x => Guid.NewGuid()).ToList();
		cards = cards.Take (Mathf.Min(cards.Count, count)).ToList();
		return cards;
	}
}

