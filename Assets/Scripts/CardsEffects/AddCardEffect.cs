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
                CardsManager.Instance.FillChooseCardField(addCardsEffect.Cards, addCardsEffect.NumberOfChosenCards, (List<CardVisual> chodenCards) =>
                {
                    List<Card> addingCards = new List<Card>();
                    foreach (CardVisual cv in chodenCards)
                    {
                        addingCards.Add(cv.CardAsset);
                    }
                    AddCardsToAim(addingCards, aims, addCardsEffect.cardsAimType);
                    if (addCardsEffect == addCardsEffects[addCardsEffects.Count - 1])
                    {
                        callback.Invoke();
                    }
                });
            }
            else
            {
                AddCardsToAim(addCardsEffect.Cards, aims, addCardsEffect.cardsAimType);
                result = true;
            }


        }

        return result;
    }

    private void AddCardsToAim(List<Card> addedCards, List<ISkillAim> aims, CardEffect.CardsAimType cardAim)
    {
        foreach (ISkillAim aim in aims)
        {
            if (aim.GetType() == typeof(PlayerVisual))
            {
                switch (cardAim)
                {
                    case CardEffect.CardsAimType.Hand:
                        foreach (Card c in addedCards)
                        {
                            NetworkCardGameManager.sInstance.AddCardToHand(c, ((PlayerVisual)aim).Player);
                        }
                        break;
                    case CardEffect.CardsAimType.Drop:
                        foreach (Card c in addedCards)
                        {
                            NetworkCardGameManager.sInstance.AddCardToDrop(c, ((PlayerVisual)aim).Player);
                        }
                        break;
                    case CardEffect.CardsAimType.Pile:
                        foreach (Card c in addedCards)
                        {
                            NetworkCardGameManager.sInstance.AddCardToPile(c, ((PlayerVisual)aim).Player);
                        }
                        break;
                }
            }
        }
    }
}
