using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellModel : MonoBehaviour {

	private GameObject model;

	public void SetColor(Color color)
	{
		foreach(MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
		{
			foreach(Material material in mr.materials)
			{
				if(material.shader == Shader.Find("Shader Forge/PlayerShader"))
				{
					material.color = new Color (color.r, color.g, color.b, material.color.a);
				}
			}
		}
	}

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
		//FindObjectOfType<Block>().GetComponentInChildren<TextMeshProUGUI> ().text = state.StateName;
	}
}
