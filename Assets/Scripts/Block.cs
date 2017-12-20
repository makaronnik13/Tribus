using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	private List<Inkome> currentIncome = new List<Inkome>();
	public List<Inkome> CurrentIncome
	{
		get
		{
			if(currentIncome.Count == 0)
			{
				foreach(Inkome inc in State.income)
				{
					currentIncome.Add (inc);
				}
			}
			return currentIncome;
		}
	}

	[SerializeField]
	private CellState state;
	public CellState State
	{
		get
		{
			return state;
		}
		set
		{
			state = value;
		}
	}

	private bool mouseInCell = false;


	void OnMouseDown()
	{
		if (State.HasCombination (SkillsController.Instance.CurrentSkill)) {
			SkillsController.Instance.ActivateSkill (this);
		}
	}

	void OnMouseUp()
	{
		BlocksField.Instance.ShowInfo (new List<Block>(){});
        InformationPanel.Instance.ShowInfo(null);
	}

	void OnMouseEnter()
	{
		mouseInCell = true;
		SkillsController.Instance.HighlightedBlock = this;
		BlocksField.Instance.HighLightFields (new List<Block>(){this});
	}

	void OnMouseExit()
	{
		mouseInCell = false;
		SkillsController.Instance.HighlightedBlock = null;
		BlocksField.Instance.HighLightFields (new List<Block>(){});
		BlocksField.Instance.ShowInfo (new List<Block>(){});
        InformationPanel.Instance.ShowInfo(null);
    }

	void OnMouseDrag()
	{
		if (mouseInCell) {
			BlocksField.Instance.ShowInfo (new List<Block> (){ this });
            InformationPanel.Instance.ShowInfo(this);
        }
	}
}
