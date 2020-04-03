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
			if(observeEffect.cardAim == CardEffect.CardAim.None || observeEffect.playerActionType!=CardEffect.CardsActionType.BurnCards)
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


		CardsManager.Instance.ChooseManager.FillChooseCardField (cards, effect.NumberOfChosenCards, (List<CardVisual> chosenCards)=>{
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

			foreach(CardVisual cv in chosenCards)
			{
				GameObject.Destroy(cv.gameObject);
			}
		});
	}

	private void BurnCards(PhotonPlayer owner, CardEffect.CardsAimType aim, List<Card> chosenCards)
	{
		switch(aim)
		{
		case CardEffect.CardsAimType.Drop:
			RPGCardGameManager.sInstance.RemoveCardsFromDrop (chosenCards, owner, true);
			break;
		case CardEffect.CardsAimType.Hand:
			RPGCardGameManager.sInstance.RemoveCardsFromHand(chosenCards, owner, true);
            break;
		case CardEffect.CardsAimType.Pile:
			RPGCardGameManager.sInstance.RemoveCardsFromPile(chosenCards, owner, true);
            break;
		case CardEffect.CardsAimType.All:
			RPGCardGameManager.sInstance.RemoveCardsFromPlayer(chosenCards, owner, true);
            break;
		}

	}

	private List<Card> GetCards(CardEffect.CardsAimType aim, PhotonPlayer owner, int count)
	{
		List<Card> cards = new List<Card> ();
		switch(aim)
		{
		case CardEffect.CardsAimType.Drop:
			cards = RPGCardGameManager.sInstance.GetPlayerDrop(owner);
			break;
		case CardEffect.CardsAimType.Hand:
			cards = RPGCardGameManager.sInstance.GetPlayerHand(owner);
                break;
		case CardEffect.CardsAimType.Pile:
			cards = RPGCardGameManager.sInstance.GetPlayerPile(owner);
                break;
		case CardEffect.CardsAimType.All:
			cards = RPGCardGameManager.sInstance.GetPlayerCards(owner);
                break;
		}

		cards = cards.OrderBy(x => Guid.NewGuid()).ToList();
		cards = cards.Take (Mathf.Min(cards.Count, count)).ToList();
		return cards;
	}
}

