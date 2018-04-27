using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HpSlider : MonoBehaviour {

	private Slider _hpSlider;
	private Slider HpSliderGui
	{
		get
		{
			if(!_hpSlider)
			{
				_hpSlider = GetComponent<Slider>();
			}
			return _hpSlider;
		}
	}
	private TextMeshProUGUI hp;
	private TextMeshProUGUI HpText
	{
		get
		{
			if(!hp)
			{
				hp = GetComponentInChildren<TextMeshProUGUI> ();
			}
			return hp;
		}
	}


	private int _hp;
	public int Hp
	{
		get
		{
			return _hp;
		}
		set
		{
			_hp = value;
			HpSliderGui.value = value;
			HpText.text = value+"/"+HpSliderGui.maxValue;
		}
	}

	public void Init(int v)
	{
		HpSliderGui.maxValue = v;
		Hp = v;
	}

}
