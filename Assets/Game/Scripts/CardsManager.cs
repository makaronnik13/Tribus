using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsManager : Singleton<CardsManager> {

	public enum ChooseType
	{
		Simple,
		Drag
	}

	public ChooseType chooseType = ChooseType.Simple;

    public GameObject CardPrefab;
    public Transform dropTransform, pileTransform, handTransform, activationSlotTransform, chooseCardField;

	public CardsLayout HandCardsLayout;
	public ChoseCardsLayout ChoseCardsLayout;

	public Action<CardVisual> OnCardTaken = (CardVisual visual)=>{};
	public Action<CardVisual> OnCardDroped = (CardVisual visual)=>{};
	public Action<CardVisual> OnCardTakenInChooseField = (CardVisual visual)=>{};

	private List<CardVisual> cardsInHand = new List<CardVisual>();
	private Action<List<CardVisual>> onChoseCardFieldClosed;
	private List<CardVisual> cardsInChoseCardField = new List<CardVisual>();
	private List<CardVisual> chosenCards
	{
		get
		{
			return ChoseCardsLayout.Instance.chosedCards;
		}
	}

	public int CardsCount
	{
		get
		{
			cardsInHand.RemoveAll (c=>c == null);
			return cardsInHand.Count;
		}
	}

	public bool CardDragging
	{
		get
		{
			foreach(CardVisual cv in HandCardsLayout.Cards)
			{
				if(cv.State == CardVisual.CardState.Dragging || cv.State == CardVisual.CardState.ChosingAim)
				{
					return true;
				}
			}
			return false;
		}
	}

    private Queue<Card> pile = new Queue<Card>();
	private Stack<Card> drop = new Stack<Card>();
    private Canvas _playerCanvas;
	public Canvas playerCanvas
    {
        get
        {
            if (!_playerCanvas)
            {
                _playerCanvas = GetComponentInParent<Canvas>();
            }
            return _playerCanvas;
        }
    }

	public void AddCardsToPile(List<Card> cards, bool shuffle = false)
	{
		Stack<Card> cardsInPile = new Stack<Card> (pile);
		foreach (Card c in cards) {
			cardsInPile.Push (c);
		}
		if(shuffle)
		{
			cardsInPile =  new Stack<Card>(cardsInPile.OrderBy (a => Guid.NewGuid ()));
		}
		pile = new Queue<Card> (cardsInPile);
	}

	public void AddCardsToDrop(List<Card> cards)
	{
		foreach (Card c in cards) {
			drop.Push (c);
		}
	}

	public void SelectPlayer(Player player)
    {
		pile = new Queue<Card>(player.Pile);
		drop = new Stack<Card>(player.Drop);

		/*Debug.Log (player.CardsInHand);
		for(int i = 0; i<player.CardsInHand;i++)
		{
			GetCard();
		}*/

		GetCard();
		for (int i = CardsManager.Instance.CardsCount; i < 5; i++)
		{
			CardsManager.Instance.GetCard();
		}
    }

	public void EndPlayerTurn(Player player)
	{
		SavePlayer (player);

		HandCardsLayout.EndTurn ();
		for(int i = cardsInHand.Count-1; i>=0;i--)
		{
			Destroy(cardsInHand[i].gameObject);
		}
	}

	public void SavePlayer(Player player)
	{
		player.Drop = drop;
		Stack<Card> cardsStack = new Stack<Card> (pile);
		//player.CardsInHand = cardsInHand.Count ();
		foreach(CardVisual cv in cardsInHand)
		{
			cardsStack.Push (cv.CardAsset);
		}
		player.Pile = new Queue<Card>(cardsStack);
	}

    public void GetCard()
    {
		if (pile.Count > 0) {
			GameObject newCard = Instantiate (CardPrefab);
			OnCardTaken.Invoke (newCard.GetComponent<CardVisual> ());
			newCard.GetComponent<CardVisual> ().Init (pile.Dequeue ());
			cardsInHand.Add (newCard.GetComponent<CardVisual> ());
			newCard.transform.SetParent (pileTransform);
			newCard.transform.localPosition = Vector3.zero;
			newCard.transform.localRotation = Quaternion.identity;
			newCard.transform.localScale = Vector3.one;
			if (pile.Count == 0) {
				Resuffle ();
			}
			newCard.transform.SetParent (handTransform);
			HandCardsLayout.CardsReposition ();
		}
    }

    public Vector3 GetPosition(CardVisual cardVisual, bool hovered = false)
    {
		return HandCardsLayout.GetPosition(cardVisual, hovered);
    }

    public Quaternion GetRotation(CardVisual cardVisual, bool hovered = false)
    {
		return HandCardsLayout.GetRotation(cardVisual, hovered);
    }

	public Vector3 GetChoosePosition(CardVisual cardVisual, bool hovered = false)
	{
		return ChoseCardsLayout.GetPosition(cardVisual, hovered);
	}

	public Quaternion GetChooseRotation(CardVisual cardVisual, bool hovered = false)
	{
		return ChoseCardsLayout.GetRotation(cardVisual, hovered);
	}

    public void DropCard(CardVisual cardVisual)
    {
			OnCardDroped.Invoke (cardVisual);
			if (!cardVisual.CardAsset.DestroyAfterPlay) 
			{
				drop.Push (cardVisual.CardAsset);
			}
			cardsInHand.Remove (cardVisual);
            Destroy(cardVisual.gameObject);
    }

    private void Resuffle()
    {
        List<Card> sorted = drop.OrderBy(a => Guid.NewGuid()).ToList();
        pile = new Queue<Card>(sorted);
        drop.Clear();
    }

	public void MoveCardTo(Transform card, Transform aim, Action callback = null)
	{
		MoveCardTo(card, aim, (CardVisual visual)=>{callback.Invoke();});
	}

	public void MoveCardTo(Transform card, Transform aim, Action<CardVisual> callback = null)
	{
		StartCoroutine (MoveCard(card, aim, callback));
	}

	IEnumerator MoveCard(Transform card, Transform aim, Action<CardVisual> callback = null)
    {
        float time = 0;
        while (card.position!=aim.position)
        {
            card.position = Vector3.Lerp(card.position, aim.position, time);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (callback!=null)
        {
			callback.Invoke(card.GetComponent<CardVisual>());
        }
    }

	public void FillChooseCardField(List<Card> cards, int max, Action<List<CardVisual>> callback = null, bool simpleCardChoose = true)
	{
		if (simpleCardChoose) 
		{
			chooseType = ChooseType.Simple;
		} else 
		{
			chooseType = ChooseType.Drag;
		}
			

		ChoseCardsLayout.Instance.Choosing = true;
		onChoseCardFieldClosed = callback;

		foreach(Card c in cards)
		{
			GameObject newCard = Instantiate (CardPrefab);
			OnCardTakenInChooseField.Invoke (newCard.GetComponent<CardVisual> ());
			newCard.GetComponent<CardVisual> ().Init (c);
			newCard.GetComponent<CardVisual> ().State = CardVisual.CardState.Choosing;
			cardsInChoseCardField.Add (newCard.GetComponent<CardVisual> ());
			newCard.transform.SetParent (chooseCardField);
			newCard.transform.localPosition = Vector3.zero;
			newCard.transform.localRotation = Quaternion.identity;
			newCard.transform.localScale = Vector3.one;
			ChoseCardsLayout.CardsReposition ();
		}
		ChoseCardsLayout.Instance.SetMax (max);
		ChoseCardsLayout.Instance.CardsReposition ();
	}

	public void HideChooseCardField()
	{
		ChoseCardsLayout.Instance.Choosing = false;
		if(onChoseCardFieldClosed!=null)
		{
			Action<List<CardVisual>> lastCallback = onChoseCardFieldClosed;
			onChoseCardFieldClosed = null;
			lastCallback(chosenCards);
		}
	}
}
