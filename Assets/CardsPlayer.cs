using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsPlayer : Singleton<CardsPlayer>
{
    public Action<Card> OnCardPlayed = (Card card) => { };

    public void PlayCard(Card card)
    {
        PlayCard(card, new List<UnityEngine.Object>());
    }

    public void PlayCard(Card card, List<UnityEngine.Object> aims)
    {
        OnCardPlayed.Invoke(card);
    }
}
