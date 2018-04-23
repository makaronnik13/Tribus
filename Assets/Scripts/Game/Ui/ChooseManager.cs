using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ChooseManager : MonoBehaviour
{
    public GameObject ButtonsVisual;
	[HideInInspector]
    public List<CardVisual> chosedCards = new List<CardVisual>();
    public TextMeshProUGUI CounterText;
    [HideInInspector]
    public int maxChose;
    public Button ApplyButton;

	private Action<List<CardVisual>> onChoseCardFieldClosed;
    private CardsLayout layout;
    public CardsLayout Layout
    {
        get
        {
            if (!layout)
            {
                layout = GetComponent<CardsLayout>();
            }
            return layout;
        }
    }

	public bool Choosing
    {
		set
        {
            ButtonsVisual.SetActive(value);
        }
		get
        {
            return ButtonsVisual.activeInHierarchy;
        }
    }
    

	private void SetMax(int max)
    {
        maxChose = max;
        CounterText.text = chosedCards.Count + "/" + maxChose;
        CounterText.enabled = (maxChose != 0);
        ApplyButton.interactable = (maxChose == 0);
		ApplyButton.gameObject.SetActive(true);
    }

    public void HideChoseCardField()
    {
        ApplyButton.interactable = false;


		foreach (CardVisual cv in Layout.Cards)
        {
			cv.OnCardVisualClicked -= CardClicked;
			if (!chosedCards.Contains(cv))
			{
				Layout.RemoveCardFromLayout(cv);
				Lean.Pool.LeanPool.Despawn(cv.gameObject);
            }
        }


        if (onChoseCardFieldClosed!=null)
		{
			Action<List<CardVisual>> lastCallback = onChoseCardFieldClosed;
			onChoseCardFieldClosed = null;
			lastCallback(chosedCards);
		}

        chosedCards.Clear();
		Choosing = false;
    }
    

	public void CardClicked(CardVisual cv)
    {
        if (chosedCards.Contains(cv))
        {
            chosedCards.Remove(cv);
            cv.AvaliabilityFrame.enabled = false;
        }
        else
        {
            if (chosedCards.Count < maxChose)
            {
                chosedCards.Add(cv);
                cv.AvaliabilityFrame.enabled = true;
            }
        }
        ApplyButton.interactable = (chosedCards.Count == maxChose);
        CounterText.text = chosedCards.Count + "/" + maxChose;
    }

	public void FillChooseCardField(List<Card> cards, int max, Action<List<CardVisual>> callback = null)
	{
		onChoseCardFieldClosed = callback;

		foreach(Card c in cards)
		{

			GameObject newCard = Lean.Pool.LeanPool.Spawn(CardsManager.Instance.CardPrefab);
			newCard.GetComponent<CardVisual> ().Init (c);
			newCard.transform.SetParent (CardsManager.Instance.topTransform);
			newCard.transform.localScale = Vector3.one;
			newCard.transform.localPosition = Vector3.one;
			newCard.transform.localRotation = Quaternion.identity;
			newCard.GetComponent<CardVisual> ().OnCardVisualClicked = CardClicked;
			CounterText.text = chosedCards.Count + "/" + maxChose;
			newCard.GetComponent<CardVisual> ().SetState(CardVisual.CardState.Choosing);
		}
		SetMax (max);
		Choosing = true;
		Layout.CardsReposition ();

        if (cards.Count <= max)
        {
            chosedCards = layout.Cards;
            HideChoseCardField();
        }
    }
		
}
