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
			switch(activeCardVisual.CardAsset.aimType)
			{
			case Card.CardAimType.Cell:
				activeCardVisual.transform.localScale = Vector3.one / 2;
				activeCardVisual.DraggingEnabled = false;
					break;
			case Card.CardAimType.Player:
				activeCardVisual.transform.localScale = Vector3.one/2;
				activeCardVisual.DraggingEnabled = false;
					break;
				case Card.CardAimType.None:
				activeCardVisual.DraggingEnabled = false;
					break;
			}
		}
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		if (activeCardVisual) 
		{
			activeCardVisual.DraggingEnabled = true;
			activeCardVisual.transform.localScale = Vector3.one;
			activeCardVisual = null;
		}
	}
}
