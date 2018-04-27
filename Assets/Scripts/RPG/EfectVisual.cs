using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EfectVisual : MonoBehaviour {

	private int stayTime = 0;
	public int StayTime
	{
		get
		{
			return stayTime;
		}
		set
		{
			stayTime = value;
			counter.text = stayTime + "";
		}
	}

	public TextMeshProUGUI counter;
	public Image img;

	private void Init(Effect effect)
	{
		img.sprite = effect.effectImage;
		stayTime = effect.Time;
	}
}
