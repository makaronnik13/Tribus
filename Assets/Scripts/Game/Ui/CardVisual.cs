using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class CardVisual : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

	public Action<CardVisual> OnCardVisualClicked = (CardVisual cv)=>{};

    public enum CardState
    {
        None,
        Hovered,
        Dragging,
        ChosingAim,
        Played,
		Choosing,
		HoveredInChoose,
		Chosed
    }

	private Camera guiCamera
	{
		get
		{
			return GUICamera.Instance.GuiCamera;
		}
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
				if (_state == CardState.Dragging) {
					if (CardCanBePlayed)
                    {
                           // MoveCardTo (CardsManager.Instance.activationSlotTransform, Vector3.zero, Quaternion.identity, Vector3.one / 2, () => {

						//});
						CardsPlayer.Instance.ActiveCard = CardAsset;
						_state = CardState.ChosingAim;
						break;
					} 
				}

					if (_state == CardState.Chosed) 
					{
						MoveCardTo (CardsManager.Instance.activationSlotTransform, Vector3.zero, Quaternion.identity, Vector3.one / 2, () => {

						});
						CardsPlayer.Instance.ActiveCard = CardAsset;
						_state = CardState.ChosingAim;
						break;
					}
						State = CardState.None;
                    break;
                case CardState.Hovered:
                    if (_state == CardState.None)
                    {
						transform.SetAsLastSibling ();
                        MoveCardTo(CardsManager.Instance.handTransform, CardsManager.Instance.GetPosition(this, true), CardsManager.Instance.GetRotation(this, true), Vector3.one*2f, () =>
                        {
							
                        });
                        _state = CardState.Hovered;
                    }
                    break;
                case CardState.None:
				if (_state == CardState.Hovered || _state == CardState.None||_state == CardState.Dragging || _state == CardState.ChosingAim)
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
				if (_state == CardState.ChosingAim) {
					CardsPlayer.Instance.ActiveCard = null;
				}
				bool dragChoosing = (CardsManager.Instance.chooseType == CardsManager.ChooseType.Drag && _state == CardState.HoveredInChoose);
				if (_state == CardState.Hovered || _state == CardState.None || _state == CardState.ChosingAim || dragChoosing) 
					{
	                    _state = CardState.Dragging;
						GetComponent<CanvasGroup> ().blocksRaycasts = false;
						transform.localScale = Vector3.one;
						transform.SetParent (CardsManager.Instance.playerCanvas.transform);
					}
	
                    break;
			case CardState.Played:
				if (_state != CardState.Played) {

					if (CardAsset.DestroyAfterPlay) 
					{
						CardsManager.Instance.DropCard (this);
					} 
					else 
					{
						MoveCardTo (CardsManager.Instance.dropTransform, Vector3.zero, Quaternion.identity, Vector3.one, () => {
							CardsManager.Instance.DropCard (this);
						});
					}
					_state = CardState.Played;
				}
                    //return card in deck
                    break;
			case CardState.Choosing:
				MoveCardTo (CardsManager.Instance.chooseCardField, CardsManager.Instance.GetChoosePosition(this), CardsManager.Instance.GetChooseRotation(this), Vector3.one, () => {
				});
				_state = CardState.Choosing;
				//return card in deck
				break;
			case CardState.HoveredInChoose:
				if (_state == CardState.Chosed || _state == CardState.Choosing) {
					transform.SetAsLastSibling ();
					MoveCardTo (CardsManager.Instance.chooseCardField, CardsManager.Instance.GetChoosePosition (this, true), CardsManager.Instance.GetChooseRotation (this, true), Vector3.one * 1.5f, () => {

					});
					_state = CardState.HoveredInChoose;
				}
				break;
			case CardState.Chosed:
				if (_state == CardState.HoveredInChoose || _state == CardState.Choosing)
				{
					MoveCardTo(CardsManager.Instance.chooseCardField, CardsManager.Instance.GetChoosePosition(this, false), CardsManager.Instance.GetChooseRotation(this, false), Vector3.one, () =>
						{

						});
					_state = CardState.Chosed;
				}

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
	public Card CardAsset
	{
		get
		{
			return _cardAsset;
		}
	}

    private bool _cardCanBePlayed = true;
	public bool CardCanBePlayed
    {
        get
        {
            return _cardCanBePlayed;
        }
        set
        {
            _cardCanBePlayed = ResourcesManager.Instance.CardAvailability(_cardAsset) && PhotonNetwork.player == NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer;
            AvaliabilityFrame.enabled = _cardCanBePlayed;
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

    [ContextMenu("push")]
    public void Push()
    {
        StartCoroutine(MoveCardToCoroutine(CardsManager.Instance.handTransform, CardsManager.Instance.GetPosition(this), CardsManager.Instance.GetRotation(this), Vector3.one, () =>
        {
        }));
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

    public void UpdateAvaliablility()
    {
        ResourceChanged(null, 0);
    }

    private void ResourceChanged(GameResource arg1, int arg2)
    {
        CardCanBePlayed = CardCanBePlayed;
    }

    #region IBeginDragHandler implementation
    public void OnBeginDrag (PointerEventData eventData)
	{
        if (LocalPlayerLogic.Instance.MyTurn)
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
			if (LocalPlayerLogic.Instance.MyTurn && eventData.pointerEnter)
            {
				Vector3 globalMousePos;
				if (RectTransformUtility.ScreenPointToWorldPointInRectangle(eventData.pointerEnter.transform as RectTransform, eventData.position, guiCamera, out globalMousePos))
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
		if (State == CardState.ChosingAim) 
		{
            CardsPlayer.Instance.PlayCard(this);
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
		if (State == CardState.Choosing || State == CardState.Chosed && !CardsManager.Instance.CardDragging) {
			State = CardState.HoveredInChoose; 
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if(State == CardState.Hovered && !CardsManager.Instance.CardDragging)
		{
        	State = CardState.None;
		}

		if(State == CardState.HoveredInChoose && !CardsManager.Instance.CardDragging)
		{
			if (GetComponentInParent<ChoseCardsLayout> ().chosedCards.Contains (this)) {
				State = CardState.Chosed;
			} else 
			{
				State = CardState.Choosing;
			}

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
        float speed = 0.25f;
		while (time<speed)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, localPosition, time/speed);
			transform.localScale = Vector3.Lerp(transform.localScale, localScale, time/speed);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, localRotation, time/speed);
            time += Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }        
        if (callback!=null)
        {
            callback.Invoke();
        }
   
        //StopAllCoroutines();
    }

	public void OnPointerClick (PointerEventData eventData)
	{
		OnCardVisualClicked.Invoke (this);
	}
}
