using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class VariantButton : MonoBehaviour {

	public GameObject DiceSidePrefab;
	public TextMeshProUGUI VariantText;
	public Transform DicesHab;

	public void Init(ChellengeVariant variant, Action<ChellengeVariant> callback)
	{
		VariantText.text = variant.text;

		DicesHab.gameObject.SetActive (variant.DiceRoll);

		if(variant.DiceRoll)
		{
			foreach(ChellengeVariant.DiceSide dice in variant.NeedDices)
			{
				GameObject newDice = Instantiate (DiceSidePrefab);
				newDice.transform.SetParent (DicesHab);
				newDice.transform.localScale = Vector3.one;
				newDice.transform.localPosition = Vector3.zero;
				newDice.GetComponent<DiceImage> ().Init (dice);
			}	
		}

		GetComponent<Button> ().onClick.AddListener (()=>{callback(variant);});

	}
}
