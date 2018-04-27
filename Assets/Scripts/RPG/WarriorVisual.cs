using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorVisual : MonoBehaviour {

    private Action callback;

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
		Super,
        Hit,
        Die
	}

	public void Animate(SkillAnimation animation, Action callback)
	{
        this.callback += callback;
		Animator.SetTrigger (animation.ToString());
	}

    public void TriggerCallback()
    {
        if (callback!=null)
        {
            callback();
            callback = null;
        }
    }
}
