﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CardsLayout : MonoBehaviour
{
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

	public int GetCardSibling(CardVisual cv)
	{
		return CardsSiblings.IndexOf(cv.transform);
	}
	public void AddCardToLayout(CardVisual visual)
	{
        if (CardsSiblings.Contains(visual.transform))
        {
            visual.transform.SetSiblingIndex(CardsSiblings.IndexOf(visual.transform));
        }

		if(!CardsSiblings.Contains(visual.transform))
		{
            visual.transform.SetParent(transform);
            CardsSiblings.Add (visual.transform);
		}
        CardsReposition();
    }
	public void RemoveCardFromLayout(CardVisual visual)
	{
       
        if (CardsSiblings.Contains(visual.transform))
        {
            visual.transform.SetParent(null);
            CardsSiblings.Remove(visual.transform);
        }
        CardsReposition();
    }
	public void CardsReposition()
	{
		foreach(Transform pair in CardsSiblings)
		{
            pair.GetComponent<CardVisual>().Reposition();
		}
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
        float cardWidth = cardSize.x;
        float offset = Mathf.Min(cardWidth, fieldWidth/cards);

        Vector3 aimPosition = Vector3.zero;
        int childId = CardsSiblings.IndexOf(cardVisual.transform);
        
        float minOffset = -(cards - 1) * offset / 2;

        float yPos = -Mathf.Pow(minOffset + childId * offset, 2) * yMultiplyer;
        aimPosition = new Vector3(minOffset+childId*offset, yPos);

        if (focused)
        {
            aimPosition += Vector3.up * cardSize.y / 2;
        }

        return aimPosition;
    }
}
