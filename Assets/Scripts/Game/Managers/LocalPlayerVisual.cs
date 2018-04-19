using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerVisual : Singleton<LocalPlayerVisual>
{
    public enum CardAnimationAim
    {
        Hand, 
        Drop,
        Pile,
        Burn,
        Top,
        Choose
    }

    public Button endTurnButton;
    public PlayersVisualizer playersVisualiser;
    public CardsLayout hand;
    public void EndTurn()
    {
        endTurnButton.interactable = false;    
    }
    public void StartTurn()
    {
        endTurnButton.interactable = true;
        FindObjectOfType<DropSlot>().ResetDrop();
    }
    public void GetCard(Card card)
    {
        CardsManager.Instance.GetCard(card);
    }

    #region cardsAnimation
    public void AddCardsToDrop(List<string> cards, Action<CardVisual> callback)
    {
        Visualize(CardAnimationAim.Top, CardAnimationAim.Drop, cards, callback);
    }

    public void AddCardsToPile(List<string> cards, Action<CardVisual> callback)
    {
        Visualize(CardAnimationAim.Top, CardAnimationAim.Pile, cards, callback);
    }

    public void AddCardsToHand(List<string> cards, Action<CardVisual> callback, CardAnimationAim from = CardAnimationAim.Top)
    {
        if (from == CardAnimationAim.Pile)
        {
            CardsManager.Instance.GetCards(cards);
            return;
        }

        Visualize(from, CardAnimationAim.Hand, cards, callback);
    }

    public void BurnCardsFromDrop(List<string> cards, Action<CardVisual> callback)
    {
        Visualize(CardAnimationAim.Drop, CardAnimationAim.Burn, cards, callback);
    }

    public void BurnCardsFromPile(List<string> cards, Action<CardVisual> callback)
    {
        Visualize(CardAnimationAim.Pile, CardAnimationAim.Burn, cards, callback);
    }

    public void BurnCardsFromHand(List<string> cards, Action<CardVisual> callback)
    {
        Visualize(CardAnimationAim.Hand, CardAnimationAim.Burn, cards, callback);
    }

    public void SteelCardsFromDrop(List<string> cards, Action<CardVisual> callback)
    {
        Visualize(CardAnimationAim.Drop, CardAnimationAim.Top, cards, callback);
    }

    public void SteelCardsFromPile(List<string> cards, Action<CardVisual> callback)
    {
        Visualize(CardAnimationAim.Pile, CardAnimationAim.Top, cards, callback);
    }

    public void SteelCardsFromHand(List<string> cards, Action<CardVisual> callback)
    {
        Visualize(CardAnimationAim.Hand, CardAnimationAim.Top, cards, callback);
    }

    private void Visualize(CardAnimationAim from, CardAnimationAim to, List<string> cards, Action<CardVisual> callback = null)
    {
        if(ChoseCardsLayout.Instance.Choosing)
        {
            from = CardAnimationAim.Choose;
        }

        StartCoroutine(MoveCardIn(from, to, cards, callback));
    }
    private IEnumerator MoveCardIn(CardAnimationAim from, CardAnimationAim to, List<string> cards, Action<CardVisual> callback = null)
    {
        Queue<string> movingCards = new Queue<string>(cards);
        List<CardVisual> createdVisuals = new List<CardVisual>();
        while (movingCards.Count>0)
        {
            Card nextCard = DefaultResourcesManager.GetCardById(movingCards.Dequeue());
            GameObject newCard = null;

            if (from != CardAnimationAim.Choose && from != CardAnimationAim.Hand)
            {
                newCard = CardsManager.Instance.CreateCard(nextCard);
                switch (from)
                {
                    case CardAnimationAim.Drop:
                        newCard.transform.SetParent(CardsManager.Instance.dropTransform);
                        newCard.transform.localScale = Vector3.one;
                        newCard.transform.localPosition = Vector3.one;
                        break;
                    case CardAnimationAim.Pile:
                        newCard.transform.SetParent(CardsManager.Instance.pileTransform);
                        newCard.transform.localScale = Vector3.one;
                        newCard.transform.localPosition = Vector3.one;
                        break;
                    case CardAnimationAim.Top:
                        newCard.transform.SetParent(CardsManager.Instance.topTransform);
                        newCard.transform.localScale = Vector3.one;
                        newCard.transform.localPosition = Vector3.one;
                        break;
                }
            }

            if(from == CardAnimationAim.Choose)
            {
                foreach (Transform t in ChoseCardsLayout.Instance.transform)
                {
                    if (t.GetComponent<CardVisual>().CardAsset == nextCard && !createdVisuals.Contains(t.GetComponent<CardVisual>()))
                    {
                        newCard = t.gameObject;
                        break;
                    }
                }
            }

            if (from == CardAnimationAim.Hand)
            {
                foreach (Transform t in CardsLayout.Instance.transform)
                {
                    if (t.GetComponent<CardVisual>().CardAsset == nextCard && !createdVisuals.Contains(t.GetComponent<CardVisual>()))
                    {
                        newCard = t.gameObject;
                        break;
                    }
                }
            }

            newCard.GetComponent<CardVisual>().SetState(CardVisual.CardState.Visualising);
            createdVisuals.Add(newCard.GetComponent<CardVisual>());
            yield return new WaitForSeconds(0.5f);
        }

        float waitTime = 0;
        while (waitTime<1)
        {
            waitTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        StartCoroutine(MoveCardOut(to, createdVisuals, callback));
    }
    private IEnumerator MoveCardOut(CardAnimationAim to, List<CardVisual> cards, Action<CardVisual> callback = null)
    {
        Queue<CardVisual> movingCards = new Queue<CardVisual>(cards);

        while (movingCards.Count > 0)
        {
            CardVisual card = movingCards.Dequeue();

            Debug.Log(to);

            switch (to)
            {
                case CardAnimationAim.Burn:
                    card.SetState(CardVisual.CardState.Burned);
                    break;
                case CardAnimationAim.Drop:
                    card.SetState(CardVisual.CardState.Played); 
                    break;
                case CardAnimationAim.Hand:
                    card.SetState(CardVisual.CardState.Hand);
                    break;
                case CardAnimationAim.Pile:
                    card.SetState(CardVisual.CardState.Piled);
                    break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion
}
