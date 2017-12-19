using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsPanel : Singleton<SkillsPanel> {

	public RectTransform SkillTransform(CombineModel.Skills skill)
	{
		foreach(SkillButton sb in GetComponentsInChildren<SkillButton>())
		{
			if(sb.skill == skill)
			{
				return sb.GetComponent<RectTransform> ();
			}
		}

		return null;
	}

	public Sprite SkillImage(CombineModel.Skills skill)
	{
		foreach(SkillButton sb in GetComponentsInChildren<SkillButton>())
		{
			if(sb.skill == skill)
			{
				return sb.GetComponent<Image> ().sprite;
			}
		}

		return null;
	}
}
