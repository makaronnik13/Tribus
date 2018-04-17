using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardView : MonoBehaviour {

	public Transform costTransform;
	public GameObject costPrefab;
	public TextMeshProUGUI CardName, CardDescription;
	public Image AvaliabilityFrame;
	public Image CardImage;
	private Card _cardAsset;
	public Card CardAsset
	{
		get
		{
			return _cardAsset;
		}
	}

	public void Init(Card card)
	{
		_cardAsset = card;
		CardName.text = card.CardName;
		CardDescription.text = card.CardDescription;
		CardImage.sprite = card.cardSprite;
		foreach (Inkome ink in card.Cost)
		{
			GameObject newVisualiser = Instantiate(costPrefab, Vector3.zero, Quaternion.identity, costTransform);
			newVisualiser.transform.localScale = Vector3.one;
			newVisualiser.GetComponent<Image>().sprite = ink.resource.sprite;
			newVisualiser.GetComponentInChildren<TextMeshProUGUI>().text = "" + ink.value;
		}
	}
}
