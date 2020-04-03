using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour {

	// Use this for initialization
	void Awake () 
	{
		PlayerStats.Instance.OnMoneyChanged += MoneyChanged;
	}
	
	private void MoneyChanged(int v)
	{
		GetComponent<TextMeshProUGUI> ().text = v + "";
	}
}
