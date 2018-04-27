using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorVisual : MonoBehaviour {


	private Animator animator;
	private Animator Animator
	{
		get
		{
			if(!animator)
			{
				animator = GetComponent<Animator> ();
			}
			return animator;
		}
	}

	public enum SkillAnimation
	{
		Atack,
		Buff,
		Heal,
		Block,
		Super
	}

	public void Animate(SkillAnimation animation)
	{
		animator.SetTrigger (animation.ToString());
	}
}
