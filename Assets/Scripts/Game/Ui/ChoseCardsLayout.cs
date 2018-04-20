using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ChoseCardsLayout : MonoBehaviour
{
    #region ccl
    public GameObject Parent;
    public List<CardVisual> chosedCards = new List<CardVisual>();
    public TextMeshProUGUI CounterText;
    public int maxChose;
    public Button ApplyButton;
    #endregion

    private List<Transform> CardsSiblings = new List<Transform>();
    private Vector2 _cardSize = Vector2.zero;
    private Vector2 cardSize
    {
        get
        {
            if (_cardSize == Vector2.zero)
            {
                _cardSize = FindObjectOfType<CardVisual>().GetComponent<RectTransform>().rect.size;
            }
            return _cardSize;
        }
    }
	public List<CardVisual> Cards
	{
		get
		{
			List<CardVisual> cv = new List<CardVisual> ();
			foreach(Transform pair in CardsSiblings)
			{
				cv.Add (pair.GetComponent<CardVisual>());
			}
			return cv;
		}
	}
	private RectTransform _rectTransform;
	private RectTransform rectTransform
	{
		get
		{
			if (!_rectTransform)
			{
				_rectTransform = GetComponent<RectTransform>();
			}
			return _rectTransform;
		}
	}

    #region ccl
    public bool Choosing
    {
        set
        {
            Parent.SetActive(value);
        }
        get
        {
            return Parent.activeInHierarchy;
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
        CardsSiblings.Clear();
        foreach (Transform t in transform)
        {
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
    #endregion


    public int GetCardSibling(CardVisual cv)
	{
		return CardsSiblings.IndexOf(cv.transform);
	}
    public void AddCardToLayout(CardVisual visual)
    {
        if (!CardsSiblings.Contains(visual.transform))
        {
            visual.transform.SetParent(transform);
            if (CardsManager.Instance.chooseType == CardsManager.ChooseType.Simple)
            {
                visual.OnCardVisualClicked += CardClicked;
            }
            CardsSiblings.Add(visual.transform);
        }
        CardsReposition();
    }
    public void RemoveCardFromLayout(CardVisual visual)
    {
        if (CardsSiblings.Contains(visual.transform))
        {
            visual.transform.SetParent(null);
            if (CardsManager.Instance.chooseType == CardsManager.ChooseType.Simple)
            {
                visual.OnCardVisualClicked -= CardClicked;
            }
            CardsSiblings.Remove(visual.transform);
        }
        CardsReposition();
    }
	public void CardsReposition()
	{
		CounterText.text = chosedCards.Count+"/"+maxChose;
        foreach (Transform pair in CardsSiblings)
        {
            pair.GetComponent<CardVisual>().Reposition();
        }
    }
    public Quaternion GetRotation(CardVisual cardVisual, bool focused = false)
    {
        return Quaternion.identity;
    }
    public Vector3 GetPosition(CardVisual cardVisual, bool focused = false)
    {
        float yMultiplyer = 1f / 10000;
        int cards = transform.childCount;
        float fieldWidth = GetComponent<RectTransform>().rect.width;
        float cardWidth = cardSize.x;
        float offset = Mathf.Min(cardWidth, fieldWidth / cards);

        Vector3 aimPosition = Vector3.zero;
        int childId = CardsSiblings.IndexOf(cardVisual.transform);

        float minOffset = -(cards - 1) * offset / 2;

        float yPos = -Mathf.Pow(minOffset + childId * offset, 2) * yMultiplyer;
        aimPosition = new Vector3(minOffset + childId * offset, yPos);

        if (focused)
        {
            aimPosition += Vector3.up * cardSize.y / 2;
        }

        return aimPosition;
    }

	
}
