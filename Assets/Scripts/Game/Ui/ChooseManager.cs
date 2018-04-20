using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ChooseManager : MonoBehaviour
{
    public GameObject ButtonsVisual;
    public List<CardVisual> chosedCards = new List<CardVisual>();
    public TextMeshProUGUI CounterText;
    [HideInInspector]
    public int maxChose;
    public Button ApplyButton;
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

    private void Awake()
    {
        Layout.OnCardAddedToLayout += (visual) =>
        {
            if (CardsManager.Instance.chooseType == CardsManager.ChooseType.Simple)
            {
                visual.OnCardVisualClicked += CardClicked;
            }
            CounterText.text = chosedCards.Count + "/" + maxChose;
        };

        Layout.OnCardAddedToLayout += (visual) =>
        {
            if (CardsManager.Instance.chooseType == CardsManager.ChooseType.Simple)
            {
                visual.OnCardVisualClicked -= CardClicked;
            }
            CounterText.text = chosedCards.Count + "/" + maxChose;
        };
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
    public void SetMax(int max)
    {
        maxChose = max;
        CounterText.text = chosedCards.Count + "/" + maxChose;
        CounterText.enabled = (maxChose != 0 && CardsManager.Instance.chooseType == CardsManager.ChooseType.Simple);
        ApplyButton.interactable = (maxChose == 0);
        ApplyButton.gameObject.SetActive(CardsManager.Instance.chooseType == CardsManager.ChooseType.Simple);
    }
    public void HideChoseCardField()
    {
        ApplyButton.interactable = false;

        foreach (Transform t in transform)
        {
            Layout.RemoveCardFromLayout(t.GetComponent<CardVisual>());
            if (!chosedCards.Contains(t.GetComponent<CardVisual>()))
            {
                Lean.Pool.LeanPool.Despawn(t.gameObject);
            }
        }
        CardsManager.Instance.HideChooseCardField();
        chosedCards.Clear();
    }
    private void CardClicked(CardVisual cv)
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
}
