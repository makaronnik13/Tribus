using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class CardVisual : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	private static float _movementSpeed = 0.3f;
    private static float _scaleSpeed = 0.2f;
    private Material _disolveMaterial;
    private Material disolveMateral
    {
        get
        {
            if (!_disolveMaterial)
            {
                _disolveMaterial = new Material(Resources.Load("Materials/CardDisolve") as Material);
            }
            return _disolveMaterial;
        }
    }

    public enum CardState
    {
        None,
        Hand,
        Hovered,
        Dragging,
        ChosingAim,
        Played,
        Choosing,
        HoveredInChoose,
        Chosed,
        Burned,
        Piled,
        Visualising,
        Stolen
    }

    #region callbacks
    public Action<CardVisual> OnCardVisualClicked = (CardVisual cv) => { };
    #endregion

    #region privateFields
    public CardState _state = CardState.None;
    private Vector3 aimPosition;
    private Card _cardAsset;
    private Camera guiCamera
    {
        get
        {
            return GUICamera.Instance.GuiCamera;
        }
    }
    private bool _cardCanBePlayed = true;
    #endregion

    #region publicFields
    public Transform costTransform;
    public GameObject costPrefab;
    public TextMeshProUGUI CardName, CardDescription;
    public Image AvaliabilityFrame;
    public Image CardImage;
    public Card CardAsset
    {
        get
        {
            return _cardAsset;
        }

    }
    public bool CardCanBePlayed
    {
        get
        {
            return _cardCanBePlayed;
        }
        set
        {
			_cardCanBePlayed = (State == CardState.Hand || State == CardState.Hovered || State == CardState.Dragging || State == CardState.ChosingAim || State == CardState.Chosed) && ResourcesManager.Instance.CardAvailability(_cardAsset) && RPGCardGameManager.sInstance.CurrentPlayer!=null && PhotonNetwork.player == RPGCardGameManager.sInstance.CurrentPlayer.photonPlayer;
            
			if(State!= CardState.Choosing && State!= CardState.Chosed && State!= CardState.HoveredInChoose)
			{
				AvaliabilityFrame.enabled = _cardCanBePlayed;
			}
        }
    }
    public CardState State
    {
        get
        {
            return _state;
        }
    }
    #endregion

    #region privateMethods
	public void UpdateAvaliablility()
    {
        ResourceChanged(null, 0);
    }
    private void ResourceChanged(GameResource arg1, int arg2)
    {
        CardCanBePlayed = CardCanBePlayed;
    }
    #endregion

    #region publicMethods
    public void SetState(CardState state, Action callback = null)
    {
		if (state == _state || _state == CardState.Played)
        {
            return;
        }
			

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        switch (state)
        {
            case CardState.ChosingAim:
                if (CardCanBePlayed)
                {
                    CardsPlayer.Instance.DraggingCard = null;
                    Vector3 startPositionn = transform.position;
                    CardsManager.Instance.ChoseCardsLayout.RemoveCardFromLayout(this);
                    transform.position = startPositionn;
                    MoveCardTo(CardsManager.Instance.activationSlotTransform, Vector3.zero, Quaternion.identity, Vector3.one * 1.5f, () => { });
                    CardsPlayer.Instance.ActiveCard = this;
                    break;
                }
                else
                {
                    state = CardState.Hand;
                }
                break;
            case CardState.Hovered:
                transform.SetAsLastSibling();
                //set parent for detect need card position
                transform.SetParent(CardsManager.Instance.handTransform);
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                MoveCardTo(CardsManager.Instance.handTransform, CardsManager.Instance.GetPosition(this, true), CardsManager.Instance.GetRotation(this, true), Vector3.one * 2f, () => {
					GetComponent<CanvasGroup>().blocksRaycasts = true;
                });
                break;
		case CardState.Hand:
            Vector3 startPosition = transform.position;
            transform.SetParent(CardsManager.Instance.handTransform);
            CardsManager.Instance.ChoseCardsLayout.RemoveCardFromLayout (this);
			CardsPlayer.Instance.DraggingCard = null;
			CardsManager.Instance.HandCardsLayout.AddCardToLayout (this);
			transform.position = startPosition;
                MoveCardTo(CardsManager.Instance.handTransform, CardsManager.Instance.GetPosition(this), CardsManager.Instance.GetRotation(this), Vector3.one, () => {
					GetComponent<CanvasGroup>().blocksRaycasts = true;
                });
                CardsManager.Instance.HandCardsLayout.AddCardToLayout(this);
				GetComponent<CanvasGroup>().blocksRaycasts = true;
                break;
            case CardState.Dragging:
                CardsPlayer.Instance.ActiveCard = null;
                StopAllCoroutines();
                transform.SetParent(GUICamera.Instance.GetComponentInChildren<Canvas>().transform);
                CardsPlayer.Instance.DraggingCard = this;
                break;
		case CardState.Played:
			startPosition = transform.position;
			CardsPlayer.Instance.ActiveCard = null;
			CardsManager.Instance.ChoseCardsLayout.RemoveCardFromLayout (this);
			CardsManager.Instance.HandCardsLayout.RemoveCardFromLayout (this);
			transform.SetParent (CardsManager.Instance.dropTransform);
			transform.position = startPosition;
                MoveCardTo(CardsManager.Instance.dropTransform, Vector3.zero, Quaternion.identity, Vector3.one, () => 
                {
					Lean.Pool.LeanPool.Despawn(gameObject);   
                });
                break;
		case CardState.Choosing:
				GetComponent<CanvasGroup>().blocksRaycasts = true;
				CardsManager.Instance.ChoseCardsLayout.AddCardToLayout(this);
				MoveCardTo(CardsManager.Instance.chooseCardField, CardsManager.Instance.GetPosition(this), CardsManager.Instance.GetRotation(this), Vector3.one, () => { });
                break;
            case CardState.HoveredInChoose:
				GetComponent<CanvasGroup>().blocksRaycasts = true;
                transform.SetAsLastSibling();
			MoveCardTo(CardsManager.Instance.chooseCardField, CardsManager.Instance.GetPosition(this, false), CardsManager.Instance.GetRotation(this, false), Vector3.one * 1.5f, () => { });
                break;
            case CardState.Chosed:
				GetComponent<CanvasGroup>().blocksRaycasts = true;
                MoveCardTo(CardsManager.Instance.chooseCardField, CardsManager.Instance.GetPosition(this, false), CardsManager.Instance.GetRotation(this, false), Vector3.one, () => { });
                break;
            case CardState.Visualising:
                CardsManager.Instance.ChoseCardsLayout.AddCardToLayout(this);
				transform.SetParent (CardsManager.Instance.chooseCardField);
                MoveCardTo(CardsManager.Instance.chooseCardField, CardsManager.Instance.GetPosition(this), CardsManager.Instance.GetRotation(this), Vector3.one, () => { });
                break;
		case CardState.Piled:
			startPosition = transform.position;
			CardsManager.Instance.ChoseCardsLayout.RemoveCardFromLayout (this);
			CardsManager.Instance.HandCardsLayout.RemoveCardFromLayout (this);
			transform.SetParent (CardsManager.Instance.pileTransform);
			transform.position = startPosition;
				MoveCardTo(CardsManager.Instance.pileTransform, Vector3.zero, Quaternion.identity, Vector3.one, () => {
					CardsManager.Instance.ChoseCardsLayout.RemoveCardFromLayout (this);
                    Destroy(gameObject);
                });
                break;
		case CardState.Burned:
            Disolve();
			break;
        case CardState.Stolen:
                startPosition = transform.position;
                CardsManager.Instance.ChoseCardsLayout.RemoveCardFromLayout(this);
                CardsManager.Instance.HandCardsLayout.RemoveCardFromLayout(this);
                transform.SetParent(CardsManager.Instance.pileTransform);
                transform.position = startPosition;
                MoveCardTo(CardsManager.Instance.topTransform, Vector3.zero, Quaternion.identity, Vector3.one, () => {
                    CardsManager.Instance.ChoseCardsLayout.RemoveCardFromLayout(this);
                    Destroy(gameObject);
                });
                break;
        }
        _state = state;
        CardCanBePlayed = CardCanBePlayed;
    }
    public void Reposition()
    {
		if(State == CardState.Hand || State == CardState.Choosing || State == CardState.Chosed || State == CardState.Hovered || State == CardState.HoveredInChoose || State == CardState.Visualising)
		{
			CardsLayout layout = CardsManager.Instance.GetCardLayout (this);
        	//MoveCardTo(CardsManager.Instance.handTransform, CardsManager.Instance.GetPosition(this), CardsManager.Instance.GetRotation(this), Vector3.one, () => { });
			MoveCardTo(layout.transform, CardsManager.Instance.GetPosition(this), CardsManager.Instance.GetRotation(this), Vector3.one, () => {});
		}
    }
    #endregion

    #region LifeCycle
    void Awake()
    {
        ResourcesManager.Instance.OnResourceValueChanged += ResourceChanged;
    }
    public void Init(Card card)
    {
        _cardAsset = card;
        CardName.text = card.CardName;
        CardDescription.text = card.CardDescription;
        CardImage.sprite = card.cardSprite;
        foreach (Transform t in costTransform)
        {
            Destroy(t.gameObject);
        }
        foreach (Inkome ink in card.Cost)
        {
            GameObject newVisualiser = Instantiate(costPrefab, Vector3.zero, Quaternion.identity, costTransform);
            newVisualiser.transform.SetParent(costTransform);
            newVisualiser.transform.localScale = Vector3.one;
            newVisualiser.GetComponent<Image>().sprite = ink.resource.sprite;
            newVisualiser.GetComponentInChildren<TextMeshProUGUI>().text = "" + ink.value;
        }
		UpdateAvaliablility ();
		_state = CardVisual.CardState.None;
    }
    void Update()
    {
        if (_state == CardState.Dragging)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 100 * _scaleSpeed/2f);
            transform.position = aimPosition;
        }    
    }
    void OnDestroy()
    {
		if(CardsManager.Instance && CardsManager.Instance.ChoseCardsLayout.Cards.Contains(this))
		{
			CardsManager.Instance.ChoseCardsLayout.RemoveCardFromLayout (this);
		}
		if(CardsManager.Instance && CardsManager.Instance.HandCardsLayout.Cards.Contains(this))
		{
			CardsManager.Instance.HandCardsLayout.RemoveCardFromLayout (this);
		}

        if (ResourcesManager.Instance)
        {
            ResourcesManager.Instance.OnResourceValueChanged -= ResourceChanged;
        }
    }
    #endregion

    #region Interaction implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
		if (CardsPlayer.Instance.DraggingCard || CardsManager.Instance.ChooseManager.Choosing)
        {
            return;
        }
        if (LocalPlayerLogic.Instance.MyTurn && _state == CardState.Hovered)
        {
            SetState(CardState.Dragging);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_state == CardState.Dragging && CardsPlayer.Instance.DraggingCard)
        {
            if (LocalPlayerLogic.Instance.MyTurn && eventData.pointerEnter)
            {
                Vector3 globalMousePos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(eventData.pointerEnter.transform as RectTransform, eventData.position, guiCamera, out globalMousePos))
                {
                    aimPosition = globalMousePos;
                }
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
		if (CardsManager.Instance.ChooseManager.Choosing)
		{
			return;
		}

        if (_state == CardState.ChosingAim)
        {
            CardsPlayer.Instance.PlayCard(this);
        }
		else if(State!= CardState.Choosing && State!=CardState.Chosed && State!=CardState.HoveredInChoose)
        {
            SetState(CardState.Hand);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
		if (State == CardState.Choosing || State == CardState.Chosed)
		{
			SetState(CardState.HoveredInChoose);
		}

        if(CardsPlayer.Instance.DraggingCard || CardsPlayer.Instance.ActiveCard)
        {
            return;
        }
        if (State == CardState.Hand)
        {
            SetState(CardState.Hovered);
        }
       
    }
    public void OnPointerExit(PointerEventData eventData)
    {
		if (_state == CardState.HoveredInChoose)
		{
			if (GetComponentInParent<ChooseManager>().chosedCards.Contains(this))
			{
				SetState(CardState.Chosed);
			}
			else
			{
				SetState(CardState.Choosing);
			}
		}

        if (CardsPlayer.Instance.DraggingCard || CardsPlayer.Instance.ActiveCard)
        {
            return;
        }

        if (_state == CardState.Hovered)
        {
            SetState(CardState.Hand);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnCardVisualClicked.Invoke(this);
    }
    #endregion

    #region Animation
    private void MoveCardTo(Transform parent, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Action callback = null)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCardToCoroutine(parent, localPosition, localRotation, localScale, callback));
    }
    private IEnumerator MoveCardToCoroutine(Transform parent, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Action callback = null)
    {
        transform.SetParent(parent);
        float time = 0.0f;
        float speed = Mathf.Max(_movementSpeed, _scaleSpeed);

        while (time < speed)
        {
            if (time<_scaleSpeed)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, localScale, time / _scaleSpeed);
            }

            if (time < _movementSpeed)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, localPosition, time / _movementSpeed);
            }

            transform.localRotation = Quaternion.Lerp(transform.localRotation, localRotation, time / speed);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
			

        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
        transform.localScale = localScale;

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    [ContextMenu("disolve")]
    private void Disolve()
    {
        foreach (Image img in GetComponentsInChildren<Image>())
        {
            img.material = disolveMateral;
        }
        StartCoroutine(Disolve(1f));
    }

    private IEnumerator Disolve(float v)
    {
        float t = v/3f;
        while (t<v)
        {           
            disolveMateral.SetFloat("_disolveValue", t/v);
            if (t>v/2)
            {
                CardDescription.color = Color.Lerp(CardDescription.color, new Color(0, 0, 0, 0), t/v);
                CardName.color = Color.Lerp(CardName.color, new Color(0, 0, 0, 0), t/v);
            }          
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        CardsManager.Instance.ChoseCardsLayout.RemoveCardFromLayout(this);
        CardsManager.Instance.HandCardsLayout.RemoveCardFromLayout(this);
        Destroy(gameObject);
    }
    #endregion
}
