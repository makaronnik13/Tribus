using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellStateFake : MonoBehaviour {
	public List<CellState> states;
	private int i = 0;
	void Update(){
		
		if(Input.GetMouseButtonDown(0))
		{
			GetComponent<CellModel> ().SetCell (states[i]);
			i++;
			if(i>=states.Count)
			{
				i = 0;
			}
		}
	}
}
