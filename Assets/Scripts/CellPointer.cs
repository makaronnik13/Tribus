using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPointer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.GetChild (0).gameObject.SetActive (false);
		SkillsController.Instance.onHighlightedBlockChanged += Changed;
	}
	
	// Update is called once per frame
	void Changed (Block block) 
	{
		transform.GetChild (0).gameObject.SetActive (false);

		if (block && block.State.HasCombination (SkillsController.Instance.CurrentSkill)) {
			CombineModel.Skills skill = SkillsController.Instance.CurrentSkill;

			if (block && skill != CombineModel.Skills.None) {
				transform.GetChild (0).gameObject.SetActive (true);
				transform.position = block.transform.position;
				GetComponentInChildren<SpriteRenderer> ().sprite = SkillsPanel.Instance.SkillImage (skill);
			}
		}
	}
}
