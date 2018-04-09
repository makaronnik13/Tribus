using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour, ISkillAim 
{
	private CellModel cellModel;
	private CellModel CellModel
	{
		get
		{
			if(cellModel == null)
			{
				Debug.Log (gameObject);
				cellModel = GetComponentInChildren<CellModel> ();
			}

			return cellModel;
		}
	}

	private CellHighlighter highLighter;
	private CellHighlighter Highlighter
	{
		get
		{
			if(highLighter == null)
			{
				highLighter = GetComponentInChildren<CellHighlighter> ();
			}

			return highLighter;
		}
	}

	private Player owner;
	public Player Owner
	{
		get
		{
			return owner;
		}
		set
		{
			owner = value;
		}
	}

	private CombineModel.Biom biom;
	public CombineModel.Biom Biom
	{
		get
		{
			return biom;
		}
		set
		{
			biom = value;
			GetComponentInChildren<ModelReplacer> ().SetModel ((int)Biom);
		}
	}
		
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
			CellModel.SetCell (state);
			if (state) {
				Biom = state.Biom;
			} else 
			{
				Biom = CombineModel.Biom.None;
			}
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

		if(State == null)
		{
			return;
		}

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
		if(Biom == CombineModel.Biom.None)
		{
			return;
		}

		if (EventSystem.current.currentSelectedGameObject && EventSystem.current.currentSelectedGameObject.layer == 5) { // UI elements getting the hit/hover
			return;
		}

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
		

    public bool IsAwaliable(Card card)
    {
		if (!card || State == null)
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
		Highlighter.Set(v && IsAwaliable(card), false);   
    }

	public void HighlightSimple(bool v)
	{
		Highlighter.Set(v, false);  
	}

    public void HighlightSelected(Card card, bool v)
    {
		Highlighter.Set(v && IsAwaliable(card), true);   
    }

	void Start()
	{
		Animator anim = GetComponent<Animator> ();
		anim.speed = UnityEngine.Random.Range (0.3f, 0.6f);
		float randomIdleStart = UnityEngine.Random.Range(0,anim.GetCurrentAnimatorStateInfo(0).length);
		anim.Play("CellFlowing", 0, randomIdleStart);
	}
}
