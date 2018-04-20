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
        Visualising
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
            _cardCanBePlayed = (State == CardState.Hand || State == CardState.Hovered || State == CardState.Dragging || State == CardState.ChosingAim) && ResourcesManager.Instance.CardAvailability(_cardAsset) && PhotonNetwork.player == NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer;
            AvaliabilityFrame.enabled = _cardCanBePlayed;
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
    private void UpdateAvaliablility()
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
        if (state == _state)
        {
            return;
        }


        GetComponent<CanvasGroup>().blocksRaycasts = false;

        switch (state)
        {
            case CardState.ChosingAim:
                if (CardCanBePlayed)
                {
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
                CardsManager.Instance.ChoseCardsLayout.RemoveCardFromLayout(this);
                CardsPlayer.Instance.DraggingCard = null;
                CardsManager.Instance.HandCardsLayout.AddCardToLayout(this);
                MoveCardTo(CardsManager.Instance.handTransform, CardsManager.Instance.GetPosition(this), CardsManager.Instance.GetRotation(this), Vector3.one, () => {
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                });
                CardsManager.Instance.HandCardsLayout.AddCardToLayout(this);
                break;
            case CardState.Dragging:
                CardsPlayer.Instance.ActiveCard = null;
                StopAllCoroutines();
                transform.SetParent(GUICamera.Instance.GetComponentInChildren<Canvas>().transform);
                CardsPlayer.Instance.DraggingCard = this;
                break;
            case CardState.Played:
                CardsPlayer.Instance.ActiveCard = null;

                MoveCardTo(CardsManager.Instance.dropTransform, Vector3.zero, Quaternion.identity, Vector3.one, () => 
                {
                    CardsManager.Instance.ChoseCardsLayout.RemoveCardFromLayout(this);
                    CardsManager.Instance.HandCardsLayout.RemoveCardFromLayout(this);
                    CardsManager.Instance.DropCard(this);
                });
                break;
            case CardState.Choosing:
                MoveCardTo(CardsManager.Instance.chooseCardField, CardsManager.Instance.GetChoosePosition(this), CardsManager.Instance.GetChooseRotation(this), Vector3.one, () => { });
                break;
            case CardState.HoveredInChoose:
                transform.SetAsLastSibling();
                MoveCardTo(CardsManager.Instance.chooseCardField, CardsManager.Instance.GetChoosePosition(this, true), CardsManager.Instance.GetChooseRotation(this, true), Vector3.one * 1.5f, () => { });
                break;
            case CardState.Chosed:
                MoveCardTo(CardsManager.Instance.chooseCardField, CardsManager.Instance.GetChoosePosition(this, false), CardsManager.Instance.GetChooseRotation(this, false), Vector3.one, () => { });
                break;
            case CardState.Visualising:
                CardsManager.Instance.ChoseCardsLayout.AddCardToLayout(this);
                MoveCardTo(CardsManager.Instance.chooseCardField, CardsManager.Instance.GetChoosePosition(this), CardsManager.Instance.GetChooseRotation(this), Vector3.one, () => { });
                break;
            case CardState.Piled:
                CardsManager.Instance.ChoseCardsLayout.RemoveCardFromLayout(this);
                MoveCardTo(CardsManager.Instance.pileTransform, CardsManager.Instance.GetChoosePosition(this), CardsManager.Instance.GetChooseRotation(this), Vector3.one, () => {
                    Destroy(gameObject);
                });
                break;
        }
        _state = state;
        CardCanBePlayed = CardCanBePlayed;
    }
    public void Reposition()
    {
        //MoveCardTo(CardsManager.Instance.handTransform, CardsManager.Instance.GetPosition(this), CardsManager.Instance.GetRotation(this), Vector3.one, () => { });
        MoveCardTo(CardsManager.Instance.handTransform, CardsManager.Instance.GetPosition(this), CardsManager.Instance.GetRotation(this), Vector3.one, () => {});
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
        foreach (Inkome ink in card.Cost)
        {
            GameObject newVisualiser = Instantiate(costPrefab, Vector3.zero, Quaternion.identity, costTransform);
            newVisualiser.transform.localScale = Vector3.one;
            newVisualiser.GetComponent<Image>().sprite = ink.resource.sprite;
            newVisualiser.GetComponentInChildren<TextMeshProUGUI>().text = "" + ink.value;
        }
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
        if (ResourcesManager.Instance)
        {
            ResourcesManager.Instance.OnResourceValueChanged -= ResourceChanged;
        }
    }
    #endregion

    #region Interaction implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CardsPlayer.Instance.DraggingCard)
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
        if (_state == CardState.ChosingAim)
        {
            CardsPlayer.Instance.PlayCard(this);
        }
        else
        {
            SetState(CardState.Hand);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(CardsPlayer.Instance.DraggingCard)
        {
            return;
        }
        if (State == CardState.Hand)
        {
            SetState(CardState.Hovered);
        }
        if (State == CardState.Choosing || State == CardState.Chosed)
        {
            SetState(CardState.HoveredInChoose);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (CardsPlayer.Instance.DraggingCard)
        {
            return;
        }

        if (_state == CardState.Hovered)
        {
            SetState(CardState.Hand);
        }

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
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnCardVisualClicked.Invoke(this);
    }
    #endregion

    #region Animation
    private void MoveCardTo(Transform parent, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Action callback = null)
    {
        Debug.Log("move "+gameObject.GetInstanceID()+" to "+parent.gameObject.name);
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
    #endregion
}
