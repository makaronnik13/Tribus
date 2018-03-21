using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Block : MonoBehaviour, ISkillAim {

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
	
	}

	void OnMouseUp()
	{
        
    }

	void OnMouseEnter()
	{
		CardsPlayer.Instance.SelectAims (this);
        InformationPanel.Instance.ShowInfo(this);
    }

	void OnMouseExit()
	{
        CardsPlayer.Instance.SelectAims(null);
        InformationPanel.Instance.ShowInfo(null);
    }

	void OnMouseDrag()
	{
		
	}

	public void RecalculateMesh(CombineModel.Biom newBiom, int side)
	{
		
	}

    public bool IsAwaliable(Card card)
    {
        if (!card)
        {
            return false;
        }
        if (card.aimType == Card.CardAimType.Cell)
        {
            foreach (Combination comb in State.Combinations)
            {
                if (comb.skill == card.skill && comb.skillLevel == card.skillLevel)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Highlight(Card card, bool v)
    {
        BlocksField.Instance.HighLightBlock(this, v && IsAwaliable(card));
    }

    public void HighlightSelected(Card card, bool v)
    {
        BlocksField.Instance.HighLightBlock(this, v && IsAwaliable(card), true);
    }
}
