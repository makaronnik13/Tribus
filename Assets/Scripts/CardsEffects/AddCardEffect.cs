using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AddCardEffect :ICardEffect
{
	public bool TryToPlayCard(List<CardEffect> addCardsEffects, List<ISkillAim> aims, Action callback)
	{
		bool result = false;

		foreach(CardEffect addCardsEffect in addCardsEffects)
		{
			if(addCardsEffect.cardAim!=CardEffect.CardAim.Player || addCardsEffect.playerActionType!=CardEffect.PlayerActionType.AddCards)
			{
				continue;
			}
				
					if(addCardsEffect.NumberOfChosenCards < addCardsEffect.Cards.Count())
					{
						CardsManager.Instance.FillChooseCardField (addCardsEffect.Cards, addCardsEffect.NumberOfChosenCards, (List<CardVisual> chodenCards)=>{
							List<Card> addingCards = new List<Card>();
							foreach(CardVisual cv in chodenCards)
							{
								addingCards.Add(cv.CardAsset);
							}
							AddCardsToAim (addingCards, aims, addCardsEffect.cardsAimType);
							if(addCardsEffect == addCardsEffects[addCardsEffects.Count -1 ])
							{
								callback.Invoke();
							}
						}); 
					}
					else
					{
						AddCardsToAim (addCardsEffect.Cards, aims, addCardsEffect.cardsAimType);
						result = true;
					}


		}

		return result;
	}

	private void AddCardsToAim(List<Card> addedCards, List<ISkillAim> aims, CardEffect.CardsAimType cardAim)
	{
		foreach (ISkillAim aim in aims) 
		{
			if (aim.GetType () == typeof(PlayerVisual)) 
			{	
		switch(cardAim)
		{
		case CardEffect.CardsAimType.Hand:
			if ((aim as PlayerVisual).Player == GameLobby.Instance.CurrentPlayer) 
			{
				CardsManager.Instance.AddCardsToPile (addedCards);

				for(int i = 0; i< addedCards.Count;i++)
				{
					CardsManager.Instance.GetCard ();
				}	
			} 
			else 
			{
				(aim as PlayerVisual).Player.CardsInHand += addedCards.Count;
				Stack<Card> cardsInPile = new Stack<Card> ((aim as PlayerVisual).Player.Pile);
				foreach (Card c in addedCards) {
					cardsInPile.Push (c);
				}
				(aim as PlayerVisual).Player.Pile = new Queue<Card> (cardsInPile);
			}
			break;
		case CardEffect.CardsAimType.Drop:
			if ((aim as PlayerVisual).Player == GameLobby.Instance.CurrentPlayer) 
			{
				CardsManager.Instance.AddCardsToDrop (addedCards);
			} 
			else 
			{
				foreach (Card c in addedCards) 
				{
					(aim as PlayerVisual).Player.Drop.Push (c);	
				}
			}
			break;
		case CardEffect.CardsAimType.Pile:
			if ((aim as PlayerVisual).Player == GameLobby.Instance.CurrentPlayer) 
			{
				CardsManager.Instance.AddCardsToPile (addedCards, true);
			} 
			else 
			{
				Stack<Card> cardsInPile = new Stack<Card> ((aim as PlayerVisual).Player.Pile);
				foreach (Card c in addedCards) 
				{
					cardsInPile.Push (c);
				}
				(aim as PlayerVisual).Player.Pile = new Queue<Card> (cardsInPile.OrderBy(a => Guid.NewGuid()));
			}
			break;
		}
			}
		}
	}
}
