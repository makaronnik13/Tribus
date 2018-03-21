using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Block : MonoBehaviour {

	public Action<CombineModel.Biom, Block> OnBiomChanged = (CombineModel.Biom biom, Block block)=>{};

	private List<Inkome> currentIncome = new List<Inkome>();
	public List<Inkome> CurrentIncome
	{
		get
		{          
			RecalculateInkome ();
			return currentIncome;
		}
	}

	[SerializeField]
    [HideInInspector]
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
    public int Radius
    {
        get
        {
			if (State.radiusType ==  CellState.RadiusType.Simple) {
				return State.radius;
			} else {
				return  Mathf.FloorToInt(currentIncome.Find (i=>i.resource == State.radiusResource).value);
				}
        }
    }

    private bool mouseInCell = false;


    public void RecalculateInkome()
    {
        currentIncome = new List<Inkome>();

        foreach (Inkome inc in State.income)
        {
            Inkome newInkome = new Inkome();
            newInkome.resource = inc.resource;
            newInkome.value = inc.value;
            currentIncome.Add(newInkome);
        }


        foreach (Condition c in State.conditions)
        {
            foreach (Inkome pair in c.rewardResources)
            {
                Inkome inc = new Inkome();
                inc.resource = pair.resource;
                inc.value = pair.value * c.ChechCondition(this);

				Debug.Log (inc.resource+" "+inc.value);

                bool exist = false;
                foreach (Inkome income in currentIncome)
                {
                    if (income.resource == inc.resource)
                    {
                        income.value += inc.value;
                        exist = true;
                    }
                }
     
                if (!exist && inc.value!=0)
                {       
                    currentIncome.Add(inc);
                }
            }
        }
    }

	void OnMouseDown()
	{
		/*
		if (State.HasCombination (SkillsController.Instance.CurrentSkill)) {
			//SkillsController.Instance.ActivateSkill (this);
		}*/
	}

	void OnMouseUp()
	{
		/*
		Debug.Log (State);
		BlocksField.Instance.ShowInfo (new List<Block>(){});
        InformationPanel.Instance.ShowInfo(null);
		RadiusVisualizer.Instance.ShowRadius (null);
		SkillsController.Instance.ActivateSkill ();
		*/
	}

	void OnMouseEnter()
	{
		CardsPlayer.Instance.SelectBlock (this);
		//SkillsController.Instance.aimBlock = this;
		//mouseInCell = true;
		//SkillsController.Instance.HighlightedBlock = this;
		//BlocksField.Instance.HighLightFields (new List<Block>(){this});
	}

	void OnMouseExit()
	{
		mouseInCell = false;
		BlocksField.Instance.HighLightFields (new List<Block>(){});
		BlocksField.Instance.ShowInfo (new List<Block>(){});
        InformationPanel.Instance.ShowInfo(null);
		RadiusVisualizer.Instance.ShowRadius (null);
    }

	void OnMouseDrag()
	{
		if (mouseInCell) 
		{
			BlocksField.Instance.ShowInfo (new List<Block> (){ this });
            InformationPanel.Instance.ShowInfo(this);
			RadiusVisualizer.Instance.ShowRadius (this);
        }
	}

	public void RecalculateMesh(CombineModel.Biom newBiom, int side)
	{
		
	}
}
