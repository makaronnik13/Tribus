using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardVisual : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

	public TextMeshProUGUI CardName, CardDescription;
    public Image CardImage;
	public static GameObject itemBeingDragged;
	public bool FocusedEnabled = false;
	private int lastSibling;
    private Card _cardAsset;
	private int takedFromSibling;
	public bool DraggingEnabled = true;

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
    }

	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		transform.localScale = Vector3.one;
		FocusedEnabled = false;
		takedFromSibling = GetComponentInParent<CardsLayout> ().GetSibling (this);
		transform.SetParent (CardsManager.Instance.playerCanvas.transform);
		itemBeingDragged = gameObject;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
		transform.localRotation = Quaternion.identity;
	}
	#endregion

	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		if (DraggingEnabled) 
		{
			transform.position = eventData.position;
		}
	}
	#endregion

	#region IEndDragHandler implementation
	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		transform.SetParent(CardsManager.Instance.handTransform);
		transform.SetSiblingIndex (takedFromSibling);
		GetComponentInParent<CardsLayout> ().RecalculateSibling ();
	}
	#endregion

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (FocusedEnabled) 
		{
			lastSibling = transform.GetSiblingIndex ();
			transform.SetAsLastSibling ();
			transform.localScale = Vector3.one * 2;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (FocusedEnabled) 
		{
			transform.SetSiblingIndex (lastSibling);
			transform.localScale = Vector3.one;
			GetComponentInParent<CardsLayout> ().RecalculateSibling ();
		}
	}
}
