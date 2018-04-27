using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ObserveEffect :ICardEffect
{
	private List<CardEffect> effects = new List<CardEffect> ();
	private Action callback;

	public bool TryToPlayCard(List<CardEffect> observeEffects, List<ISkillAim> aims, Action callback)
	{
		this.callback = callback;
		effects = observeEffects;
		foreach(CardEffect observeEffect in observeEffects)
		{
			if(observeEffect.cardAim!=CardEffect.CardAim.Player || observeEffect.playerActionType!=CardEffect.PlayerActionType.WatchCards)
			{
				continue;
			}
				
			if(aims.Count>0)
			{
				PhotonPlayer aimPlayer = (aims[0] as PlayerVisual).Player;
				List<PlayerVisual> stayedPlayers = new List<PlayerVisual>();
				foreach(ISkillAim isa in aims)
				{
					stayedPlayers.Add (isa as PlayerVisual);
				}
				stayedPlayers.RemoveAt (0);
				Watch (aimPlayer, observeEffect, stayedPlayers);
			}
				
		}

		return false;
	}

	private void Watch(PhotonPlayer owner, CardEffect effect, List<PlayerVisual> stayedPlayers)
	{

		CardEffect.CardsAimType aim = effect.cardsAimType;
		int count = effect.NumberOfCards;

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


		Debug.Log (cards.Count);

		CardsManager.Instance.ChooseManager.FillChooseCardField (cards, 0, (List<CardVisual> chodenCards)=>{

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
}
