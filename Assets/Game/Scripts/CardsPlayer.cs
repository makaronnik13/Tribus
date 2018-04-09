using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardsPlayer : Singleton<CardsPlayer>
{
	public Action<List<ISkillAim>> OnAimsChanged = (List<ISkillAim> aims)=>{};
	private List<ICardEffect> cardEffects = new List<ICardEffect> ();

    private Card activeCard;
    public Card ActiveCard
    {
        get
        {
            return activeCard;
        }
        set
        {
            activeCard = value;

            foreach (ISkillAim aim in InterfaceHelper.FindObjects<ISkillAim>())
            {
                aim.Highlight(value, value!=null);
                aim.HighlightSelected(value, false);
            }

			if (activeCard) {
				CardEffect playerAimedCard = activeCard.CardEffects.FirstOrDefault (ce => ce.cardAim == CardEffect.CardAim.Player);
				if (playerAimedCard != null) 
				{
					foreach (PlayerVisual pv in FindObjectsOfType<PlayerVisual>()) {
						if (pv.Player == GameLobby.Instance.CurrentPlayer && playerAimedCard.playerAimType == CardEffect.PlayerAimType.You) 
						{
							SelectAim(pv);
						}
					}

					if (playerAimedCard.playerAimType == CardEffect.PlayerAimType.All) 
					{
						ISkillAim[] aims = FindObjectsOfType<PlayerVisual> ();
						SelectAims (aims);
					}

					if (playerAimedCard.playerAimType == CardEffect.PlayerAimType.Enemies) 
					{
						ISkillAim[] aims = FindObjectsOfType<PlayerVisual> ().Where(pv=>pv.Player!=GameLobby.Instance.CurrentPlayer).ToArray();
						SelectAims (aims);
					}
				}

				CardEffect cellAimedCard = activeCard.CardEffects.FirstOrDefault (ce => ce.cardAim == CardEffect.CardAim.Cell);
				if (cellAimedCard != null) 
				{
					if(cellAimedCard.cellAimType == CardEffect.CellAimType.All  || cellAimedCard.cellAimType == CardEffect.CellAimType.Random)
					{
						ISkillAim[] aims = FindObjectsOfType<Block> ().Where(b=>BlockOwnerAlowed(b, cellAimedCard)).ToArray();
						SelectAims (aims);
					}
				}
			}
        }
    }

	private bool BlockOwnerAlowed(Block block, CardEffect ce)
	{
		if(ce.cellOwnership == CardEffect.CellOwnership.Neutral && block.Owner != null)
		{
			return false;
		}
		if(ce.cellOwnership == CardEffect.CellOwnership.Player && block.Owner != GameLobby.Instance.CurrentPlayer)
		{
			return false;
		}
		if(ce.cellOwnership == CardEffect.CellOwnership.Oponent && (block.Owner == GameLobby.Instance.CurrentPlayer || block.Owner == null))
		{
			return false;
		}
		if(ce.cellOwnership == CardEffect.CellOwnership.PlayerAndNeutral && (block.Owner != GameLobby.Instance.CurrentPlayer && block.Owner!=null))
		{
			return false;
		}

		if(ce.cellOwnership == CardEffect.CellOwnership.OponentAndNeutral && block.Owner == GameLobby.Instance.CurrentPlayer)
		{
			return false;
		}

		if(ce.biomsFilter.Contains(block.Biom))
		{
			return false;
		}

		if(ce.statesFilter.Contains(block.State))
		{
			return false;
		}

		if (ce.cellActionType == CardEffect.CellActionType.Evolve) 
		{
			if(block.State == null)
			{
				return false;
			}
			foreach (Combination comb in block.State.Combinations) {
				if (comb.skill == ce.EvolveType && comb.skillLevel == ce.EvolveLevel) 
				{
					return true;
				}
			}
			return false;
		}

		return true;
	}

	public List<ISkillAim> focusedAims = new List<ISkillAim> ();

    public Action<Card> OnCardPlayed = (Card card) => { };

    public void PlayCard(CardVisual card)
    {
		
		if(card.CardAsset.CardEffects.FirstOrDefault(e=>e.cardAim != CardEffect.CardAim.None)!=null)
        {
            if (focusedAims.Count > 0)
            {
                PlayCard(card, focusedAims);
            }
            else
            {
                card.State = CardVisual.CardState.None;
            }
            
        }
        else
        {
            PlayCard(card, focusedAims);
            card.State = CardVisual.CardState.Played;
        }

        ActiveCard = null;
    }

	private void PlayCard(CardVisual card, List<ISkillAim> aims)
    {

		CardEffect cellAimedCard = activeCard.CardEffects.FirstOrDefault (ce => ce.cardAim == CardEffect.CardAim.Cell);
		if (cellAimedCard != null) 
		{
			if(cellAimedCard.cellAimType == CardEffect.CellAimType.Random)
			{
				ISkillAim[] shuffledAims = aims.OrderBy (a => Guid.NewGuid ()).ToArray ();
				SelectAims (shuffledAims.Take(Mathf.Min(cellAimedCard.NumberOfCells, shuffledAims.Length)).ToArray());
				aims = this.focusedAims;
			}
		}
			
        ActiveCard = null;

		CardsManager.Instance.OnCardDroped.Invoke (card);

		bool cardShouldBePlayed = false;

		foreach(ICardEffect cardEffect in cardEffects)
		{
			cardShouldBePlayed |= cardEffect.TryToPlayCard (card.CardAsset.CardEffects, aims, ()=>{
						OnCardPlayed.Invoke (card.CardAsset);
						CardsFieldTrigger.Instance.activeCardVisual.State = CardVisual.CardState.Played;
			});
		}	

		if (cardShouldBePlayed) 
		{
			OnCardPlayed.Invoke (card.CardAsset);
			CardsFieldTrigger.Instance.activeCardVisual.State = CardVisual.CardState.Played;
		}
    }

	public void SelectAim(ISkillAim aim)
	{
		ISkillAim[] aims = new ISkillAim[]{ aim };
		if(activeCard)
		{
		CardEffect cellAimedCard = activeCard.CardEffects.FirstOrDefault (ce => ce.cardAim == CardEffect.CardAim.Cell);
		if (cellAimedCard != null) 
		{
			if(cellAimedCard.cellAimType == CardEffect.CellAimType.Circle)
			{
					List<ISkillAim> targetBlocks = new List<ISkillAim> ();
				aims = BlocksField.Instance.GetBlocksInRadius (aim as Block, cellAimedCard.Radius).ToArray();
					foreach(Block bf in aims)
					{
						if(CellAllowed(cellAimedCard, bf))
						{
							targetBlocks.Add (bf);
						}
					}
					aims = targetBlocks.ToArray();
			}
		}
		}
		SelectAims (aims);
	}

	private bool CellAllowed(CardEffect ce, Block block)
	{
		if(ce.biomsFilter.Contains(block.Biom))
		{
			return false;
		}

		if(ce.statesFilter.Contains(block.State))
		{
			return false;
		}

		if (ce.cellActionType == CardEffect.CellActionType.Evolve) {
			if (block.State == null) {
				return false;
			}
			foreach (Combination comb in block.State.Combinations) {
				if (comb.skill == ce.EvolveType && comb.skillLevel == ce.EvolveLevel) {
					return true;
				}
			}
			return false;
		}
		return true;
	}

	public void SelectAims(ISkillAim[] aims)
	{
        foreach (ISkillAim lastTaim in focusedAims)
        {
			if (!ActiveCard) 
			{
				lastTaim.HighlightSimple (false);
			} 
			else 
			{
				lastTaim.HighlightSelected(ActiveCard, false);
			}
        }

		focusedAims = new List<ISkillAim> ();

		foreach (ISkillAim aim in aims) 
		{
			if (ActiveCard && aim != null && aim.IsAwaliable (ActiveCard)) {
				focusedAims.Add (aim);
				aim.HighlightSelected (ActiveCard, true);
			}
			
			if (!ActiveCard && aim != null) 
			{	
				focusedAims.Add (aim);
				aim.HighlightSimple (true);
			}
		}
		OnAimsChanged.Invoke (focusedAims);
    }

	void Start()
	{
		cardEffects.Add (new ChangeStateEffect());
		cardEffects.Add (new ChangeBiomEffect());
		cardEffects.Add (new DestroyStateEffect());
		cardEffects.Add (new MakeNeutralCardEffect());
		cardEffects.Add (new MakeOwnerEffect());
		cardEffects.Add (new DeevolveEffect());
		cardEffects.Add (new AddCardEffect());
		cardEffects.Add (new TakeCardEffect());
		cardEffects.Add (new ObserveEffect());
		cardEffects.Add (new BurnEffect());
		cardEffects.Add (new DropEffect());
		cardEffects.Add (new StillEffect());
		cardEffects.Add (new ChooseEffect());
	}
}
