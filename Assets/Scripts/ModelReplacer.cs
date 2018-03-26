using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelReplacer : MonoBehaviour {

	public void SetModel(int id)
	{
		int i = 0;
		foreach(Transform t in transform)
		{
			t.gameObject.SetActive (i == id);
			i++;
		}
	}
}
