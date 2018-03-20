using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillsController : Singleton<SkillsController> 
{
	public AudioClip activationSound;

	private CombineModel.Skills currentSkill;
	public Action<CombineModel.Skills> onSkillChanged;
	public CombineModel.Skills CurrentSkill
	{
		get
		{
			return currentSkill;
		}
		set
		{
			currentSkill = value;
			if(onSkillChanged!=null)
			{
				onSkillChanged.Invoke (currentSkill);
			}
			if(CurrentSkill == CombineModel.Skills.None)
			{
				HighlightedBlock = null;
			}
		}
	}

	public Action<Block> onHighlightedBlockChanged;
	private Block highlightedBlock;
	public Block HighlightedBlock
	{
		get
		{
			return highlightedBlock;
		}
		set
		{
			highlightedBlock = value;


			if(onHighlightedBlockChanged!=null)
			{
				onHighlightedBlockChanged.Invoke (highlightedBlock);
			}
		}
	}

	public void ActivateSkill(Block aimBlock)
	{
		GameObject newBlock = Instantiate(aimBlock.State.CombinationResult (SkillsController.Instance.CurrentSkill).prefab);

		newBlock.transform.SetParent (aimBlock.transform.parent);
		newBlock.transform.position = aimBlock.transform.position;
		newBlock.transform.rotation = aimBlock.transform.rotation;
		newBlock.transform.localScale = aimBlock.transform.localScale;
        newBlock.GetComponent<Block>().State = aimBlock.State.CombinationResult(SkillsController.Instance.CurrentSkill);


        Destroy (aimBlock.gameObject);
	
		CurrentSkill = CombineModel.Skills.None;
		HighlightedBlock = null;
        foreach (Block b in FindObjectsOfType<Block>())
        {
            b.RecalculateInkome();
        }
		GetComponent<AudioSource> ().PlayOneShot (activationSound);
    }

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			CurrentSkill = CombineModel.Skills.None;
		}
	}
}
