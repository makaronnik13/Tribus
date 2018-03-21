using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class CardVisual : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum CardState
    {
        None,
        Hovered,
        Dragging,
        ChosingAim,
        Played
    }

    private CardState _state = CardState.None;
    public CardState State
    {
        get
        {
            return _state;
        }
        set
        {
            switch (value)
            {
                case CardState.ChosingAim:
                    if (_state == CardState.Dragging)
                    {
					if (CardCanBePlayed) {
						MoveCardTo (CardsManager.Instance.activationSlotTransform, Vector3.zero, Quaternion.identity, Vector3.one / 2, () => {

						});
						_state = CardState.ChosingAim;
					} else 
					{
						State = CardState.None;
					}

                    }
                    break;
                case CardState.Hovered:
                    if (_state == CardState.None)
                    {
						transform.SetAsLastSibling ();
                        MoveCardTo(CardsManager.Instance.handTransform, CardsManager.Instance.GetPosition(this, true), CardsManager.Instance.GetRotation(this, true), Vector3.one*2, () =>
                        {
							
                        });
                        _state = CardState.Hovered;
                    }
                    break;
                case CardState.None:
				if (_state == CardState.Hovered || _state == CardState.None||_state == CardState.Dragging)
                    {
						GetComponent<CanvasGroup>().blocksRaycasts = true;
						transform.SetParent (CardsManager.Instance.handTransform);
						transform.SetSiblingIndex(GetComponentInParent<CardsLayout>().GetCardSibling(this));
                        MoveCardTo(CardsManager.Instance.handTransform, CardsManager.Instance.GetPosition(this), CardsManager.Instance.GetRotation(this), Vector3.one, ()=>
                        {	
                        });
                        _state = CardState.None;
                        //make card small and return in hand
                    }
                    break;
			case CardState.Dragging:
				if (_state == CardState.Hovered || _state == CardState.None || _state == CardState.ChosingAim) {
					_state = CardState.Dragging;
					GetComponent<CanvasGroup> ().blocksRaycasts = false;
					transform.localScale = Vector3.one;
					transform.SetParent (CardsManager.Instance.playerCanvas.transform);
				}
	
                    break;
			case CardState.Played:
				MoveCardTo (CardsManager.Instance.dropTransform, Vector3.zero, Quaternion.identity, Vector3.one, () => {
					CardsManager.Instance.DropCard (this);
				});
				CardsPlayer.Instance.PlayCard (this);
						_state = CardState.Played;
                    //return card in deck
                    break;
            }
        }
    }

    public Transform costTransform;
    public GameObject costPrefab;
	public TextMeshProUGUI CardName, CardDescription;
    public Image AvaliabilityFrame;
    public Image CardImage;
    private Card _cardAsset;

    private bool _cardCanBePlayed = true;
    private bool CardCanBePlayed
    {
        get
        {
            return _cardCanBePlayed;
        }
        set
        {
            _cardCanBePlayed = ResourcesManager.Instance.CardAvailability(_cardAsset);
            AvaliabilityFrame.enabled = _cardCanBePlayed;
        }
    }

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

        State = CardState.None;
    }

    void Awake()
    {
        ResourcesManager.Instance.OnResourceValueChanged += ResourceChanged;
    }

    void OnDestroy()
    {
		if(ResourcesManager.Instance)
		{
        	ResourcesManager.Instance.OnResourceValueChanged -= ResourceChanged;
		}
    }

    private void ResourceChanged(GameResource arg1, int arg2)
    {
        CardCanBePlayed = CardCanBePlayed;
    }

    #region IBeginDragHandler implementation
    public void OnBeginDrag (PointerEventData eventData)
	{
        if (FakeController.Instance.MyTurn)
        {
            State = CardState.Dragging;
        }
	}
	#endregion

	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
        if (State == CardState.Dragging)
        {
			if (FakeController.Instance.MyTurn && eventData.pointerEnter)
            {
				Vector3 globalMousePos;
				if (RectTransformUtility.ScreenPointToWorldPointInRectangle(eventData.pointerEnter.transform as RectTransform, eventData.position, Camera.main, out globalMousePos))
				{
					transform.localScale = Vector3.one;
					transform.position = globalMousePos;
				}
            }
        }
	}
	#endregion

	#region IEndDragHandler implementation
	public void OnEndDrag (PointerEventData eventData)
	{
		if (State == CardState.ChosingAim) {
			State = CardState.Played;
		} else {
			State = CardState.None;
		}/*
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.SetParent(CardsManager.Instance.handTransform);
            transform.SetSiblingIndex(takedFromSibling);
            GetComponentInParent<CardsLayout>().RecalculateSibling();
            */
	}
	#endregion

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (State == CardState.None && !CardsManager.Instance.CardDragging) {
			State = CardState.Hovered; 
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if(State == CardState.Hovered && !CardsManager.Instance.CardDragging)
		{
        	State = CardState.None;
		}
    }

    private void MoveCardTo(Transform parent, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Action callback = null)
    {
		StopCoroutine(MoveCardToCoroutine(parent, localPosition, localRotation, localScale, callback));
        StartCoroutine(MoveCardToCoroutine(parent, localPosition, localRotation, localScale, callback));
    }

    IEnumerator MoveCardToCoroutine(Transform parent, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Action callback = null)
    {
        transform.SetParent(parent);
        float time = 0;
		while (time<1)
		{	
			transform.localPosition = Vector3.Lerp(transform.localPosition, localPosition, time);
			transform.localScale = Vector3.Lerp(transform.localScale, localScale, time);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, localRotation, time);
            time += Time.deltaTime*4;
            yield return new WaitForEndOfFrame();
        }
        if (callback!=null)
        {
            callback.Invoke();
        }
    }
}
