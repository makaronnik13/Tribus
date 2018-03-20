using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler 
{
    public Action<int> OnDropCountsChanged = (int c) => { };

    private int _awaliableDrops = 3;
    private int awaliableDrops
    {
        get
        {
            return _awaliableDrops;
        }
        set
        {
            _awaliableDrops = value;
            OnDropCountsChanged.Invoke(_awaliableDrops);
        }
    }

    [ContextMenu("Reset")]
    public void ResetDrop()
    {
        awaliableDrops = 3;
    }

	public void OnDrop (PointerEventData eventData)
	{
		if(awaliableDrops>0)
		{
			DropCard (eventData.pointerDrag.GetComponent<CardVisual>());
		}
	}

	public void DropCard(CardVisual visual)
    {   
		visual.transform.SetParent (transform);
		visual.transform.localPosition = Vector3.zero;
			CardsManager.Instance.DropCard(visual);
            awaliableDrops--;
    }
}
