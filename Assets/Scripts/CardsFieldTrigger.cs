using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsFieldTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private CardVisual activeCardVisual;

	public void OnPointerEnter (PointerEventData eventData)
	{
		if(!eventData.pointerDrag)
		{
			return;
		}

		activeCardVisual = eventData.pointerDrag.GetComponent<CardVisual> ();

		if(activeCardVisual)
		{
            activeCardVisual.State = CardVisual.CardState.ChosingAim;
        }
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		if (activeCardVisual && (activeCardVisual.State == CardVisual.CardState.Dragging || activeCardVisual.State == CardVisual.CardState.ChosingAim)) 
		{
            activeCardVisual.State = CardVisual.CardState.Dragging;
			activeCardVisual = null;
		}
	}
}
