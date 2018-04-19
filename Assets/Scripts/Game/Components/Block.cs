using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class Block : Photon.MonoBehaviour, ISkillAim 
{
	private CellModel cellModel;
	private CellModel CellModel
	{
		get
		{
			if(cellModel == null)
			{
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

	private PhotonPlayer owner;
	public PhotonPlayer Owner
	{
		get
		{
			return owner;
		}
		set
		{
			owner = value;


			if (owner==null) {
				if(GetComponentInChildren<CellModel>()!=null)
				{
					GetComponentInChildren<CellModel> ().SetColor (Color.white);
				}
			} else 
			{
                if (GetComponentInChildren<CellModel>()!=null)
				{
					GetComponentInChildren<CellModel> ().SetColor (NetworkCardGameManager.sInstance.GetPlayerColor(owner));
				}
			}
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
            if (biom!=value)
            {
                GetComponentInChildren<ModelReplacer>().SetModel((int)value);

                if (value == CombineModel.Biom.None)
                {
                    State = null;
                }
                else
                {
                    foreach (CellState cs in BlocksField.Instance.baseStates)
                    {
                        if (cs && cs.Biom == value)
                        {
                            State = cs;
                        }
                    }    
                }
                biom = value;
                CellModel.SetCell(State);
                CellModel.SetCell(State);
                RecalculateInkome();
            }	    
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
    //[HideInInspector]
	public CellState state;
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
			if(BlocksField.Instance.baseStates.Contains(state))
			{
				Owner = null;
			}
			RecalculateInkome ();
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
		if (EventSystem.current.currentSelectedGameObject && EventSystem.current.currentSelectedGameObject.layer == 5) 
		{ // UI elements getting the hit/hover
			return;
		}

		CameraController.Instance.AimedBlockChanged (this);

		if(CardsPlayer.Instance.ActiveCard)
		{
			CardEffect cardEffect = CardsPlayer.Instance.ActiveCard.CardAsset.CardEffects.FirstOrDefault (ce=>ce.cardAim == CardEffect.CardAim.Player);
			if(cardEffect!=null)
			{
				if (cardEffect.playerAimType == CardEffect.PlayerAimType.All || cardEffect.playerAimType == CardEffect.PlayerAimType.Enemies || cardEffect.playerAimType == CardEffect.PlayerAimType.You) 
				{
					return;
				}
			}


			cardEffect = CardsPlayer.Instance.ActiveCard.CardAsset.CardEffects.FirstOrDefault (ce=>ce.cardAim == CardEffect.CardAim.Cell);
			if(cardEffect!=null)
			{
				if(cardEffect.cellAimType == CardEffect.CellAimType.All  || cardEffect.cellAimType == CardEffect.CellAimType.Random)
				{
					return;
				}
			}
		}

		CardsPlayer.Instance.SelectAim (this);
        InformationPanel.Instance.ShowInfo(this);
    }
	void OnMouseExit()
	{
		bool shouldDehighlight = true;

		if (CardsPlayer.Instance.ActiveCard) {
			CardEffect cardEffect = CardsPlayer.Instance.ActiveCard.CardAsset.CardEffects.FirstOrDefault (ce => ce.cardAim == CardEffect.CardAim.Player);
			if (cardEffect != null) {
				if (cardEffect.playerAimType == CardEffect.PlayerAimType.All || cardEffect.playerAimType == CardEffect.PlayerAimType.Enemies || cardEffect.playerAimType == CardEffect.PlayerAimType.You) {
					shouldDehighlight = false;
				}
			}

			cardEffect = CardsPlayer.Instance.ActiveCard.CardAsset.CardEffects.FirstOrDefault (ce=>ce.cardAim == CardEffect.CardAim.Cell);
			if(cardEffect!=null)
			{
				if(cardEffect.cellAimType == CardEffect.CellAimType.All  || cardEffect.cellAimType == CardEffect.CellAimType.Random)
				{
					shouldDehighlight = false;
				}
			}
		}
			
		if(shouldDehighlight)
		{
			CardsPlayer.Instance.SelectAim (null);
		}
			
        InformationPanel.Instance.ShowInfo(null);
    }
	void OnMouseDrag()
	{
		
	}
    public bool IsAwaliable(Card card)
    {
		if (!card)
        {
            return false;
        }

		if(card.CardEffects.Count() == 0)
		{
			return false;
		}

		CardEffect ce = card.CardEffects.FirstOrDefault(cardEffect=>cardEffect.cardAim == CardEffect.CardAim.Cell); 

		if (ce!=null)
        {
			if(ce.cellOwnership == CardEffect.CellOwnership.Neutral && Owner != null)
			{
				return false;
			}
			if(ce.cellOwnership == CardEffect.CellOwnership.Player && Owner != NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer)
			{
				return false;
			}
			if(ce.cellOwnership == CardEffect.CellOwnership.Oponent && (Owner == NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer || Owner == null))
			{
				return false;
			}
			if(ce.cellOwnership == CardEffect.CellOwnership.PlayerAndNeutral && (Owner != NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer && Owner!=null))
			{
				return false;
			}

			if(ce.cellOwnership == CardEffect.CellOwnership.OponentAndNeutral && Owner == NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer)
			{
				return false;
			}

			if(ce.biomsFilter.Contains(Biom))
			{
				return false;
			}

			if(ce.statesFilter.Contains(State))
			{
				return false;
			}

			if (ce.cellActionType == CardEffect.CellActionType.Evolve) 
			{
				if(State == null)
				{
					return false;
				}
				foreach (Combination comb in State.Combinations) {
					if (comb.skill == ce.EvolveType && comb.skillLevel == ce.EvolveLevel) 
					{
						return true;
					}
				}
				return false;
			}

			return true;
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

    [PunRPC]
    public void InitBlock(float[] localPos, int randomRotation, int state)
    {
        transform.SetParent(BlocksField.Instance.transform);
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.Euler(Vector3.up * 60 * randomRotation);
        transform.localPosition = new Vector3(localPos[0], localPos[2], localPos[1]);

        if (BlocksField.Instance.baseStates[state])
        {
            Biom = BlocksField.Instance.baseStates[state].Biom;
        }
        else
        {
            Biom = CombineModel.Biom.None;
        }

        State = BlocksField.Instance.baseStates[state];
    }

    [PunRPC]
    private void RpcChangeState(int stateId)
    {
        State = DefaultResourcesManager.AllStatesList.States[stateId];
    }
    [PunRPC]
    private void RpcChangeOwner(PhotonPlayer player)
    {
        Owner = player;
    }

    [PunRPC]
    private void RpcChangeBiom(CombineModel.Biom biom)
    {
        Biom = biom;
    }
}
