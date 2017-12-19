using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {

	public CombineModel.Skills skill;
	private float time;

	public bool Awaliable = true;

	void Start()
	{
		GetComponent<Button> ().onClick.AddListener(PushButton);
	}

	void PushButton()
	{
		if (SkillsController.Instance.CurrentSkill == skill) 
		{
			SkillsController.Instance.CurrentSkill = CombineModel.Skills.None;
		} else 
		{
			SkillsController.Instance.CurrentSkill = skill;
		}
		//ReloadButton ();
	}

	public void ReloadButton()
	{
		GetComponent<Button> ().interactable = false;
		time = 0;
	}

	void Update()
	{
		time += Time.deltaTime;
		transform.GetChild (0).GetComponent<Image> ().fillAmount = time/CombineModel.SkillCd (skill);
		if(time>=CombineModel.SkillCd(skill))
		{
			if(Awaliable)
			{
				GetComponent<Button> ().interactable = true;
			}
		}
	}
}
