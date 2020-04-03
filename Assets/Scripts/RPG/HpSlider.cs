using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HpSlider : MonoBehaviour {

	public Image sliderImg;

	private int maxValue;
	private Material _hpSlider;
	private Material HpSliderGui
	{
		get
		{
			if(!_hpSlider)
			{
				_hpSlider = new Material(sliderImg.material);
				sliderImg.material = _hpSlider;
			}
			return _hpSlider;
		}
	}

    public TextMeshProUGUI HpText;
	


	private int _hp;
	public int Hp
	{
		get
		{
			return _hp;
		}
		set
		{
			GetComponentInParent<WarriorObject> ().EmmitParticle (_hp - value, false);
			_hp = value;
			StopCoroutine (SetValue(_hp));
			StartCoroutine (SetValue(_hp));
			HpText.text = value+"/"+maxValue;
		}
	}

	public void Init(int v)
	{
		maxValue = v;
		_hp = v;
		HpText.text = _hp+"/"+maxValue;
		StartCoroutine (SetValue(_hp));
	}

	private IEnumerator SetValue(float v)
	{
		float startValue = HpSliderGui.GetFloat("_Value");
		float t = 0;
		while(t<0.8f)
		{
			
			float val = Mathf.Lerp (startValue, (v+0.0f)/maxValue, t/0.8f);
			HpSliderGui.SetFloat("_Value", val);
			t += Time.deltaTime;
			yield return new WaitForSeconds (Time.deltaTime);
		}
	}
}
