using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EfectVisual : MonoBehaviour {

	private float stayTime = 0;
	public float StayTime
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

	private void Init(Effect effect, float time)
	{
		img.sprite = effect.effectImage;
        StayTime = time;
	}
}
