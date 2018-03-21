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
			Debug.Log ("inst");
			GameObject newBlock = Instantiate(cs.prefab);

			newBlock.transform.SetParent (aimBlock.transform.parent);
			newBlock.transform.position = aimBlock.transform.position;
			newBlock.transform.rotation = aimBlock.transform.rotation;
			newBlock.transform.localScale = aimBlock.transform.localScale;

			newBlock.GetComponent<Block>().State = cs;


	        Destroy (aimBlock.gameObject);

	        foreach (Block b in FindObjectsOfType<Block>())
	        {
	            //b.RecalculateInkome();
	        }
			GetComponent<AudioSource> ().PlayOneShot (activationSound);
		}
    }
}
