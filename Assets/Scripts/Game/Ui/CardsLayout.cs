﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CardsLayout : MonoBehaviour
{
	private List<Transform> CardsSiblings = new List<Transform>();
	public Action OnCardsReposition = ()=>{};

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

	void Awake()
	{
		CardsManager.Instance.OnCardTaken += CardTaken;
		CardsManager.Instance.OnCardDroped += CardDroped;
	}

	public int GetCardSibling(CardVisual cv)
	{
		return CardsSiblings.IndexOf(cv.transform);
	}

	private void CardTaken(CardVisual visual)
	{
		CardsSiblings.Add (visual.transform);
		CardsReposition ();
	}

	private void CardDroped(CardVisual visual)
	{
		CardsSiblings.Remove (visual.transform);
		CardsReposition ();
	}

	public void CardsReposition()
	{
		foreach(Transform pair in CardsSiblings)
		{
			pair.GetComponent<CardVisual> ().State = pair.GetComponent<CardVisual> ().State;
			pair.GetComponent<CardVisual> ().CardCanBePlayed = pair.GetComponent<CardVisual> ().CardCanBePlayed;
		}

		OnCardsReposition.Invoke ();
	}

    public Quaternion GetRotation(CardVisual cardVisual, bool focused = false)
    {
		int cards = transform.childCount;
        float rotOffset = 3;
        float maxRot = 20;

        Quaternion aimRotation = Quaternion.identity;
        float offset = Mathf.Min(rotOffset, maxRot / cards);
        int childId = CardsSiblings.IndexOf(cardVisual.transform);
        float minOffset = -(cards - 1) * offset / 2;
        float rot = (minOffset + childId * offset);

        if (!focused)
        {
            aimRotation = Quaternion.Euler(new Vector3(0, 0, -rot));
        }

        return aimRotation;
    }

    public Vector3 GetPosition(CardVisual cardVisual, bool focused = false)
    {
        float yMultiplyer = 1f / 10000;
        int cards = transform.childCount;
        float fieldWidth = GetComponent<RectTransform>().rect.width;
        float cardWidth = transform.GetChild(0).GetComponent<RectTransform>().rect.width;
        float cardHeight = transform.GetChild(0).GetComponent<RectTransform>().rect.height;
        float offset = Mathf.Min(cardWidth, fieldWidth/cards);

        Vector3 aimPosition = Vector3.zero;
        int childId = CardsSiblings.IndexOf(cardVisual.transform);
        
        float minOffset = -(cards - 1) * offset / 2;

        float yPos = -Mathf.Pow(minOffset + childId * offset, 2) * yMultiplyer;
        aimPosition = new Vector3(minOffset+childId*offset, yPos);

        if (focused)
        {
            aimPosition += Vector3.up * transform.GetChild(0).GetComponent<RectTransform>().rect.height / 2;
        }

        return aimPosition;
    }

    IEnumerator MoveTo(Transform card, Vector3 position, Quaternion rotation)
    {
        float time = 0;
        while (card.transform.localPosition != position|| card.transform.localRotation != rotation)
        {
            card.localRotation =  Quaternion.Lerp(card.localRotation, rotation, time);
            card.localPosition = Vector3.Lerp(card.localPosition, position, time);
            time += Time.deltaTime*2;

            yield return new WaitForEndOfFrame();
        }
    }

	public void EndTurn()
	{
		CardsSiblings.Clear ();
	}
}