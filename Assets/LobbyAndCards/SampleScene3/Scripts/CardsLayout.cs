using System.Collections;
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
		OnCardsReposition += ()=>{CardsManager.Instance.SavePlayer (GameLobby.Instance.CurrentPlayer);};
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
        float minRotation = -20;

        Quaternion aimRotation = Quaternion.identity;

		foreach (Transform t in transform)
        {
            if (t == cardVisual.transform)
            {
				float rotation = Mathf.Lerp(-minRotation, minRotation,  CardsSiblings.IndexOf(t) / (cards + 0.0f));
                aimRotation = Quaternion.Euler(new Vector3(0, 0, rotation));

                if (focused)
                {
                    aimRotation = Quaternion.identity;
                }
            }
        }

            return aimRotation;
    }

    public Vector3 GetPosition(CardVisual cardVisual, bool focused = false)
    {
		int cards = transform.childCount;
        Vector3 aimPosition = Vector3.zero;
        Vector3 minPosition = new Vector3(GetComponent<RectTransform>().rect.width / 2, 0, 0);
		foreach (Transform t in transform)
        {
            if (t == cardVisual.transform)
            {
				aimPosition = Vector3.Lerp(-minPosition, minPosition, CardsSiblings.IndexOf(t) / (cards + 0.0f));

                if (focused)
                {
                    aimPosition += Vector3.up * t.GetComponent<RectTransform>().rect.height / 2;
                }
            }
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
