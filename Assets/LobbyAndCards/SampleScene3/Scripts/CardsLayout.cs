using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CardsLayout : MonoBehaviour
{
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
	private int childCount = 0;

	private List<Transform> childTransforms = new List<Transform>();

	public void Update()
	{
		if(childCount!=transform.childCount)
		{
			RecalculateSibling ();
			childCount = transform.childCount;
		}

        //LayoutCards();
	}

	public void RecalculateSibling()
	{
		childTransforms.Clear ();
		foreach(Transform t in transform)
		{
			childTransforms.Add (t);
		}
        foreach (Transform t in childTransforms)
        {
            t.GetComponent<CardVisual>().State = t.GetComponent<CardVisual>().State;
        }
	}

	// Update is called once per frame
	void LayoutCards() 
	{
			int i = 0;
			int cards = transform.childCount;
			float minRotation = -20;
			Vector3 minPosition = new Vector3 (GetComponent<RectTransform>().rect.width/2, 0,0);
			foreach(Transform t in childTransforms)
			{
				float rotation = Mathf.Lerp(-minRotation, minRotation, i/(cards+0.0f));
				Quaternion aimRotation = Quaternion.Euler (new Vector3 (0, 0, rotation));
				Vector3 aimPosition = Vector3.Lerp (-minPosition, minPosition, i / (cards + 0.0f));

				if(t.localScale!=Vector3.one)
				{
					aimRotation = Quaternion.identity;
					aimPosition += Vector3.up * t.GetComponent<RectTransform>().rect.height/2;
				}

	
				if (aimPosition != t.localPosition || aimRotation != t.localRotation)
                {
                    StopCoroutine(MoveTo(t, aimPosition, aimRotation));
                    StartCoroutine(MoveTo(t, aimPosition, aimRotation));
				}
				i++;
			}
	}

    public Quaternion GetRotation(CardVisual cardVisual, bool focused = false)
    {
        int i = 0;
        int cards = transform.childCount;
        float minRotation = -20;

        Quaternion aimRotation = Quaternion.identity;

        foreach (Transform t in childTransforms)
        {
            if (t == cardVisual.transform)
            {
                float rotation = Mathf.Lerp(-minRotation, minRotation, i / (cards + 0.0f));
                aimRotation = Quaternion.Euler(new Vector3(0, 0, rotation));

                if (focused)
                {
                    aimRotation = Quaternion.identity;
                }
            }
            i++;
        }

            return aimRotation;
    }

    public Vector3 GetPosition(CardVisual cardVisual, bool focused = false)
    {
        int i = 0;
        int cards = transform.childCount;
        Vector3 aimPosition = Vector3.zero;
        Vector3 minPosition = new Vector3(GetComponent<RectTransform>().rect.width / 2, 0, 0);
        foreach (Transform t in childTransforms)
        {
            if (t == cardVisual.transform)
            {
                aimPosition = Vector3.Lerp(-minPosition, minPosition, i / (cards + 0.0f));

                if (focused)
                {
                    aimPosition += Vector3.up * t.GetComponent<RectTransform>().rect.height / 2;
                }
            }
            i++;
        }

        return aimPosition;
    }

    public int GetSibling(CardVisual cv)
	{
		for(int i = 0; i<childTransforms.Count;i++)
		{
			if(childTransforms[i].GetComponent<CardVisual>()==cv)
			{
				return i;
			}
		}
		return 0;
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
}
