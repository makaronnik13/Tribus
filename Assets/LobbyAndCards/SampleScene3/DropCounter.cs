using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropCounter : MonoBehaviour {

	public TextMeshProUGUI counterText;
	public Image backgroundImg;
	public DropSlot slot;

	// Use this for initialization
	void Start () 
	{
		slot.OnDropCountsChanged += CounterChanged;
	}

    private void CounterChanged(int obj)
    {
        counterText.text = "" + obj;
        backgroundImg.enabled = (obj!=0);
        counterText.enabled = (obj != 0);
    }
}
