using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AddCardEffect : ICardEffect
{
    public bool TryToPlayCard(List<CardEffect> addCardsEffects, List<ISkillAim> aims, Action callback)
    {
        bool result = false;

        foreach (CardEffect addCardsEffect in addCardsEffects)
        {
            if (addCardsEffect.cardAim != CardEffect.CardAim.Player || addCardsEffect.playerActionType != CardEffect.PlayerActionType.AddCards)
            {
                continue;
            }

            if (addCardsEffect.NumberOfChosenCards < addCardsEffect.Cards.Count() && addCardsEffect.NumberOfChosenCards!=0)
            {
				CardsManager.Instance.ChooseManager.FillChooseCardField(addCardsEffect.Cards, addCardsEffect.NumberOfChosenCards, (List<CardVisual> chodenCards) =>
                {
                    List<Card> addingCards = new List<Card>();
                    foreach (CardVisual cv in chodenCards)
                    {
                        addingCards.Add(cv.CardAsset);
                    }

                    AddCardsToAim(addingCards, aims, addCardsEffect.cardsAimType, true);
                    if (addCardsEffect == addCardsEffects[addCardsEffects.Count - 1])
                    {
                        callback.Invoke();
                    }
                });
            }
            else
            {
                AddCardsToAim(addCardsEffect.Cards, aims, addCardsEffect.cardsAimType, false);
                result = true;
            }


        }

        return result;
    }

	private void AddCardsToAim(List<Card> addedCards, List<ISkillAim> aims, CardEffect.CardsAimType cardAim, bool dontWait)
    {
        foreach (ISkillAim aim in aims)
        {
            if (aim.GetType() == typeof(PlayerVisual))
            {
				LocalPlayerVisual.CardAnimationAim animationAim = LocalPlayerVisual.CardAnimationAim.Top;

				if(dontWait)
				{
					animationAim = LocalPlayerVisual.CardAnimationAim.Choose;
				}

                switch (cardAim)
                {
                    case CardEffect.CardsAimType.Hand:
					RPGCardGameManager.sInstance.AddCardsToHand(addedCards, ((PlayerVisual)aim).Player, animationAim, true, dontWait);
                        break;
                    case CardEffect.CardsAimType.Drop:
					RPGCardGameManager.sInstance.AddCardsToDrop(addedCards, ((PlayerVisual)aim).Player, animationAim, true, dontWait);
                        break;
                    case CardEffect.CardsAimType.Pile:         
					RPGCardGameManager.sInstance.AddCardsToPile(addedCards, ((PlayerVisual)aim).Player, animationAim, true, dontWait);
                        break;
                }
            }
        }
    }
}
