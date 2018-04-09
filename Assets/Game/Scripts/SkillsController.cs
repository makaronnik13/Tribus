using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillsController : Singleton<SkillsController> 
{
	public AudioClip activationSound;

	public void ActivateSkill(Block aimBlock, CombineModel.Skills skill, int skillLevel)
	{
		CellState cs = aimBlock.State.CombinationResult (skill,skillLevel);
		if(cs)
		{
			aimBlock.State = cs;
			aimBlock.Owner = GameLobby.Instance.CurrentPlayer;
			GetComponent<AudioSource> ().PlayOneShot (activationSound);
		}
    }
}
