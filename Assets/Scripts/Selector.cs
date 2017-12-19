using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SkillsController.Instance.onSkillChanged += Changed;	
	}

	void Changed(CombineModel.Skills c)
	{
		if (c == CombineModel.Skills.None) {
			GetComponent<Image> ().enabled = false;
		} else 
		{
			GetComponent<Image> ().enabled = true;

		

			GetComponent<RectTransform> ().position = SkillsPanel.Instance.SkillTransform (c).position;

		}
	}
}
