using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsManager : Singleton<CardsManager> {

    public GameObject CardPrefab;
    public Transform dropTransform, pileTransform, handTransform, activationSlotTransform, chooseCardField, topTransform;

    private CardsLayout handCardsLayout;
    public CardsLayout HandCardsLayout
    {
        get
        {
            if (!handCardsLayout)
            {
                handCardsLayout = handTransform.GetComponent<CardsLayout>();
            }
            return handCardsLayout;
        }
    }

    private ChooseManager chooseManager;
    public ChooseManager ChooseManager
    {
        get
        {
            if (!chooseManager)
            {
                chooseManager = chooseCardField.GetComponent<ChooseManager>();
            }
            return chooseManager;
        }
    }

    public CardsLayout ChoseCardsLayout
    {
        get
        {
            return ChooseManager.Layout;
        }
    }

	public Action<CardVisual> OnCardTaken = (CardVisual visual)=>{};
	public Action<CardVisual> OnCardDroped = (CardVisual visual)=>{};
	public Action<CardVisual> OnCardTakenInChooseField = (CardVisual visual)=>{};

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

 
    public GameObject CreateCard(Card card)
    {
        GameObject newCard = Lean.Pool.LeanPool.Spawn(CardPrefab);
        newCard.GetComponent<CardVisual>().Init(card);
        return newCard;
    }

    public void GetCards(List<string> cards)
    {
        foreach (string card in cards)
        {
            GetCard(DefaultResourcesManager.GetCardById(card));
        }
    }

    public void GetCard(Card card)
    {
        GameObject newCard = CreateCard(card);
        newCard.transform.SetParent(pileTransform);
        newCard.transform.localPosition = Vector3.zero;
	    newCard.transform.localRotation = Quaternion.identity;
	    newCard.transform.localScale = Vector3.one;
        newCard.GetComponent<CardVisual>().SetState(CardVisual.CardState.Hand);
    }

	public CardsLayout GetCardLayout(CardVisual visual)
	{
		if(ChoseCardsLayout.Cards.Contains(visual))
		{
			return ChoseCardsLayout;
		}
		if(HandCardsLayout.Cards.Contains(visual))
		{
			return HandCardsLayout;
		}
		throw new UnityException("card is not on layout. trying to reposition"); 
		return null;
	}

    public Vector3 GetPosition(CardVisual cardVisual, bool hovered = false)
    {
		return GetCardLayout(cardVisual).GetPosition(cardVisual, hovered);
    }

    public Quaternion GetRotation(CardVisual cardVisual, bool hovered = false)
    {
		return GetCardLayout(cardVisual).GetRotation(cardVisual, hovered);
    }
		
    public void DropCard(CardVisual cardVisual)
    {
		cardVisual.SetState (CardVisual.CardState.Played);
        CardsPlayer.Instance.DraggingCard = null;
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
		
}
