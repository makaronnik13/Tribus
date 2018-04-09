using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellModel : MonoBehaviour {

	private GameObject model;

	public void SetCell(CellState state)
	{
		if(model)
		{
			Destroy(model);
		}

		if (state && state.prefab) 
		{
			model = Instantiate (state.prefab);
			model.transform.SetParent (transform);
			model.transform.localRotation = Quaternion.identity;
			model.transform.localPosition = Vector3.zero;
			model.transform.localScale = Vector3.one;
		}
	}
}
