using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsPlayer : Singleton<CardsPlayer>
{
	public Action<List<ISkillAim>> OnAimsChanged = (List<ISkillAim> aims)=>{};

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
        }
    }

	public List<ISkillAim> focusedAims = new List<ISkillAim> ();

    public Action<Card> OnCardPlayed = (Card card) => { };

    public void PlayCard(CardVisual card)
    {
        if(card.CardAsset.aimType!= Card.CardAimType.None)
        {
            if (focusedAims.Count > 0)
            {
                PlayCard(card, focusedAims);
                card.State = CardVisual.CardState.Played;
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
        ActiveCard = null;
		CardsManager.Instance.OnCardDroped.Invoke (card);
		foreach(ISkillAim aim in aims)
		{
			if(aim.GetType()==typeof(Block))
			{
				SkillsController.Instance.ActivateSkill (aim as Block, card.CardAsset.skill, card.CardAsset.skillLevel);
			}
		}
        card.State = CardVisual.CardState.Played;
		OnCardPlayed.Invoke(card.CardAsset);
    }


	public void SelectAims(ISkillAim aim)
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

		if (ActiveCard && aim!=null && aim.IsAwaliable(ActiveCard))
        {
            focusedAims.Add(aim);
            foreach (ISkillAim lastTaim in focusedAims)
            {
                lastTaim.HighlightSelected(ActiveCard, true);
            }
        }
			
		if(!ActiveCard && aim!=null)
		{	
			focusedAims.Add (aim);

			foreach (ISkillAim lastTaim in focusedAims)
			{
				lastTaim.HighlightSimple(true);
			}
		}

		OnAimsChanged.Invoke (focusedAims);
    }
}
