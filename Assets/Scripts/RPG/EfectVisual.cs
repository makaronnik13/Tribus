using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EfectVisual : MonoBehaviour {

	public Effect Modifier;
	private WarriorObject warrior;

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
			counter.text = Mathf.RoundToInt(stayTime) + "";
		}
	}

	public TextMeshProUGUI counter;
	public Image img;

	public void Init(Effect effect, float time)
	{
		Modifier = effect;
		img.sprite = effect.effectImage;
        StayTime = time;
		warrior = GetComponentInParent<WarriorObject> ();
	}

	public void Activate()
	{
		if(Modifier.addHpEveryTurn!=0)
		{
			float hpChange = Modifier.addHpEveryTurn;
			if(Modifier.multiplyOnLength)
			{
				hpChange *= StayTime;
			}
			warrior.RecieveDamage (Mathf.CeilToInt (hpChange));	
		}
	}
}
