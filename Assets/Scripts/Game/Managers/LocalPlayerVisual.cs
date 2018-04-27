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
    }
    public void GetCard(Card card)
    {
        CardsManager.Instance.GetCard(card);
    }

    #region cardsAnimation
	public void AddCardsToDrop(List<string> cards, Action<CardVisual> callback, CardAnimationAim from = CardAnimationAim.Top, bool dontWait = false)
    {
		Visualize(from, CardAnimationAim.Drop, cards, callback, dontWait);
    }

	public void AddCardsToPile(List<string> cards, Action<CardVisual> callback, CardAnimationAim from = CardAnimationAim.Top, bool dontWait = false)
    {
		Visualize(from, CardAnimationAim.Pile, cards, callback, dontWait);
    }

	public void AddCardsToHand(List<string> cards, Action<CardVisual> callback, CardAnimationAim from = CardAnimationAim.Top, bool dontWait = false)
    {
        if (from == CardAnimationAim.Pile)
        {
            CardsManager.Instance.GetCards(cards);
            return;
        }
        Visualize(from, CardAnimationAim.Hand, cards, callback, dontWait);
    }

	public void BurnCardsFromDrop(string[] cards, Action<CardVisual> callback = null)
    {
		Visualize(CardAnimationAim.Drop, CardAnimationAim.Burn, cards.ToList(), callback, true, false);
    }

	public void BurnCardsFromPile(string[] cards, Action<CardVisual> callback = null)
    {
		Visualize(CardAnimationAim.Pile, CardAnimationAim.Burn, cards.ToList(), callback, true, false);
    }

	public void BurnCardsFromHand(string[] cards, Action<CardVisual> callback = null)
    {
		Visualize(CardAnimationAim.Hand, CardAnimationAim.Burn, cards.ToList(), callback, true, false);
    }

	public void SteelCardsFromDrop(string[] cards, Action<CardVisual> callback = null)
    {
		Visualize(CardAnimationAim.Drop, CardAnimationAim.Top, cards.ToList(), callback);
    }

	public void SteelCardsFromPile(string[] cards, Action<CardVisual> callback = null)
    {
		Visualize(CardAnimationAim.Pile, CardAnimationAim.Top, cards.ToList(), callback);
    }

	public void SteelCardsFromHand(string[] cards, Action<CardVisual> callback = null)
    {
		Visualize(CardAnimationAim.Hand, CardAnimationAim.Top, cards.ToList(), callback);
    }

	private void Visualize(CardAnimationAim from, CardAnimationAim to, List<string> cards, Action<CardVisual> callback = null, bool dontWait = false, bool outDelay = true)
    {
        StartCoroutine(MoveCardIn(from, to, cards, callback, dontWait, outDelay));
    }
	private IEnumerator MoveCardIn(CardAnimationAim fromAim, CardAnimationAim to, List<string> cards, Action<CardVisual> callback = null, bool dontWait = false, bool outDelay = true)
    {
        Queue<string> movingCards = new Queue<string>(cards);
        List<CardVisual> createdVisuals = new List<CardVisual>();

		if (fromAim == CardAnimationAim.Choose) {
			createdVisuals = new List<CardVisual>(CardsManager.Instance.ChooseManager.chosedCards);
			movingCards.Clear ();
		}

		if (fromAim == CardAnimationAim.Hand) 
		{
			foreach (string movingCard in movingCards) 
			{
				foreach (CardVisual visual in CardsManager.Instance.HandCardsLayout.Cards) 
				{
					if(!createdVisuals.Contains(visual) && visual.CardAsset.name == movingCard)
					{
						createdVisuals.Add(visual);
						break;
					}
				}
			}
				
			movingCards.Clear ();
		}
			

		while (movingCards.Count > 0) {
			Card nextCard = DefaultResourcesManager.GetCardById (movingCards.Dequeue ());
			GameObject newCard = null;

				newCard = CardsManager.Instance.CreateCard (nextCard);
				switch (fromAim) {
				case CardAnimationAim.Drop:
					newCard.transform.SetParent (CardsManager.Instance.dropTransform);
					break;
				case CardAnimationAim.Pile:
					newCard.transform.SetParent (CardsManager.Instance.pileTransform);
					break;
				case CardAnimationAim.Top:
					newCard.transform.SetParent (CardsManager.Instance.topTransform);
					break;
				}
				newCard.transform.localScale = Vector3.one;
				newCard.transform.localPosition = Vector3.one;
				newCard.transform.localRotation = Quaternion.identity;
			createdVisuals.Add (newCard.GetComponent<CardVisual> ());
		}
			

		for(int i = 0; i<createdVisuals.Count; i++)
		{
			createdVisuals[i].SetState (CardVisual.CardState.Visualising);	
			yield return new WaitForSeconds (0.2f);
		}

        float waitTime = 0;
		while (waitTime<1 && !dontWait)
        {
            waitTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
		StartCoroutine(MoveCardOut(to, createdVisuals, callback, outDelay));
    }

	private IEnumerator MoveCardOut(CardAnimationAim to, List<CardVisual> cards, Action<CardVisual> callback = null, bool withDelay = true)
    {

        Queue<CardVisual> movingCards = new Queue<CardVisual>(cards);

        while (movingCards.Count > 0)
        {
            CardVisual card = movingCards.Dequeue();
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
				    Debug.Log ("chose to pile");
                        card.SetState(CardVisual.CardState.Piled);
                        break;
                case CardAnimationAim.Top:
                    card.SetState(CardVisual.CardState.Stolen);
                    break;
            }

			if(withDelay)
			{
				yield return new WaitForSeconds(0.5f);
			}
        }
    }
    #endregion
}
