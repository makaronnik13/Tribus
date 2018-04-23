using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardsPlayer : Singleton<CardsPlayer>
{
	public Action<List<ISkillAim>> OnAimsChanged = (List<ISkillAim> aims)=>{};
	private List<ICardEffect> cardEffects = new List<ICardEffect> ();

    public CardVisual DraggingCard;
    private CardVisual activeCard;
	public CardVisual ActiveCard
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
				Card card = null;
				if(value)
				{
					card = value.CardAsset;
				}
				aim.Highlight(card, value!=null);
				aim.HighlightSelected(card, false);
            }

			if (activeCard) {
				CardEffect playerAimedCard = activeCard.CardAsset.CardEffects.FirstOrDefault (ce => ce.cardAim == CardEffect.CardAim.Player);
				if (playerAimedCard != null) 
				{
					foreach (PlayerVisual pv in FindObjectsOfType<PlayerVisual>()) {
						if (pv.Player == NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer && playerAimedCard.playerAimType == CardEffect.PlayerAimType.You) 
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
						ISkillAim[] aims = FindObjectsOfType<PlayerVisual> ().Where(pv=>pv.Player!= NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer).ToArray();
						SelectAims (aims);
					}
				}

				CardEffect cellAimedCard = activeCard.CardAsset.CardEffects.FirstOrDefault (ce => ce.cardAim == CardEffect.CardAim.Cell);
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
		if(ce.cellOwnership == CardEffect.CellOwnership.Player && block.Owner != NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer)
		{
			return false;
		}
		if(ce.cellOwnership == CardEffect.CellOwnership.Oponent && (block.Owner == NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer || block.Owner == null))
		{
			return false;
		}
		if(ce.cellOwnership == CardEffect.CellOwnership.PlayerAndNeutral && (block.Owner != NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer && block.Owner!=null))
		{
			return false;
		}

		if(ce.cellOwnership == CardEffect.CellOwnership.OponentAndNeutral && block.Owner == NetworkCardGameManager.sInstance.CurrentPlayer.photonPlayer)
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
        if (card.CardAsset.CardEffects.FirstOrDefault(e=>e.cardAim != CardEffect.CardAim.None)!=null)
        {
            if (focusedAims.Count > 0)
            {
                PlayCard(card, focusedAims);
            }
            else
            {
                card.SetState(CardVisual.CardState.Hand);
            }
            
        }
        else
        {
            PlayCard(card, focusedAims);
        }

        ActiveCard = null;
    }

    private void PlayCard(CardVisual card, List<ISkillAim> aims)
    {
        CardEffect cellAimedCard = activeCard.CardAsset.CardEffects.FirstOrDefault(ce => ce.cardAim == CardEffect.CardAim.Cell);
        if (cellAimedCard != null)
        {
            if (cellAimedCard.cellAimType == CardEffect.CellAimType.Random)
            {
                ISkillAim[] shuffledAims = aims.OrderBy(a => Guid.NewGuid()).ToArray();
                SelectAims(shuffledAims.Take(Mathf.Min(cellAimedCard.NumberOfCells, shuffledAims.Length)).ToArray());
                aims = this.focusedAims;
            }
        }

        ActiveCard = null;

        NetworkCardGameManager.sInstance.RemoveCardFromHand(card.CardAsset, PhotonNetwork.player);

        if (card.CardAsset.DestroyAfterPlay)
        {
            card.SetState(CardVisual.CardState.Burned);
        }
        else
        {
            NetworkCardGameManager.sInstance.AddCardToDrop(card.CardAsset, PhotonNetwork.player, LocalPlayerVisual.CardAnimationAim.Hand, false);
            CardsManager.Instance.DropCard(card);
        }

		OnCardPlayed.Invoke (card.CardAsset);

		foreach(ICardEffect cardEffect in cardEffects)
		{
			cardEffect.TryToPlayCard (card.CardAsset.CardEffects, aims, ()=> { });
		}
    }

	public void SelectAim(ISkillAim aim)
	{
		ISkillAim[] aims = new ISkillAim[]{ aim };
		if(activeCard)
		{
			CardEffect cellAimedCard = activeCard.CardAsset.CardEffects.FirstOrDefault (ce => ce.cardAim == CardEffect.CardAim.Cell);
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
				lastTaim.HighlightSelected(ActiveCard.CardAsset, false);
			}
        }

		focusedAims = new List<ISkillAim> ();

		foreach (ISkillAim aim in aims) 
		{
			if (ActiveCard && aim != null && aim.IsAwaliable (ActiveCard.CardAsset)) {
				focusedAims.Add (aim);
				aim.HighlightSelected (ActiveCard.CardAsset, true);
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
		//base
		cardEffects.Add (new ChangeStateEffect());
		cardEffects.Add (new ChangeBiomEffect());
		cardEffects.Add (new DestroyStateEffect());
		cardEffects.Add (new MakeNeutralCardEffect());
		cardEffects.Add (new MakeOwnerEffect());
		cardEffects.Add (new DeevolveEffect());

		//cards
		cardEffects.Add (new AddCardEffect());
		cardEffects.Add (new TakeCardEffect());
		cardEffects.Add (new ObserveEffect());
		cardEffects.Add (new BurnEffect());
		cardEffects.Add (new DropEffect());
		cardEffects.Add (new StillEffect());

		//additional
		cardEffects.Add (new ChooseEffect());
	}
}
