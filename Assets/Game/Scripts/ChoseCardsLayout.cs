using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ChoseCardsLayout : Singleton<ChoseCardsLayout> {

	private List<Transform> CardsSiblings = new List<Transform>();
	public Action OnCardsReposition = ()=>{};
	public GameObject Parent;
	public List<CardVisual> chosedCards = new List<CardVisual> ();
	public TextMeshProUGUI CounterText;
	public int maxChose;
	public Button ApplyButton;

	public void SetMax(int max)
	{
		maxChose = max;
		CounterText.text = chosedCards.Count+"/"+maxChose;
		CounterText.enabled = (maxChose != 0 && CardsManager.Instance.chooseType == CardsManager.ChooseType.Simple);
		ApplyButton.interactable = (maxChose == 0);
		ApplyButton.gameObject.SetActive(CardsManager.Instance.chooseType == CardsManager.ChooseType.Simple);
	}

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
		CardsManager.Instance.OnCardTakenInChooseField += CardTaken;
		//CardsManager.Instance.OnCardDroped += CardDroped;
		//OnCardsReposition += ()=>{CardsManager.Instance.SavePlayer (GameLobby.Instance.CurrentPlayer);};
	}

	public int GetCardSibling(CardVisual cv)
	{
		return CardsSiblings.IndexOf(cv.transform);
	}

	private void CardTaken(CardVisual visual)
	{
		if (CardsManager.Instance.chooseType == CardsManager.ChooseType.Simple) 
		{
			visual.OnCardVisualClicked += CardClicked;
		}
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
		}

		OnCardsReposition.Invoke ();
		CounterText.text = chosedCards.Count+"/"+maxChose;
	}

	public Quaternion GetRotation(CardVisual cardVisual, bool focused = false)
	{
		int cards = transform.childCount;
		float minRotation = 0;

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
					aimPosition += Vector3.up * t.GetComponent<RectTransform> ().rect.height / 4;
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

	public void HideChoseCardField()
	{
		ApplyButton.interactable = false;
			CardsSiblings.Clear ();
			foreach(Transform t in transform)
			{
			if(!chosedCards.Contains(t.GetComponent<CardVisual>()))
			{
				Destroy(t.gameObject);
			}
			}
		CardsManager.Instance.HideChooseCardField ();
		chosedCards.Clear ();
	}

	private void CardClicked(CardVisual cv)
	{
		if (chosedCards.Contains (cv)) {
			chosedCards.Remove (cv);
			cv.AvaliabilityFrame.enabled = false;
		}
		else 
		{
			if (chosedCards.Count < maxChose) 
			{
				chosedCards.Add (cv);
				cv.AvaliabilityFrame.enabled = true;
			}
		}
		ApplyButton.interactable = (chosedCards.Count == maxChose);
		CounterText.text = chosedCards.Count+"/"+maxChose;
	}
}
