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
				CardEffect playerAimedCard = activeCard.CardAsset.CardEffects.FirstOrDefault (ce => ce.cardAim != CardEffect.CardAim.None);
				if (playerAimedCard != null) 
				{
                    List<ISkillAim> aims = new List<ISkillAim>();
					foreach (WarriorObject w in FindObjectsOfType<WarriorObject>())
                    {
                        switch (playerAimedCard.cardAim)
                        {
                            case CardEffect.CardAim.All:      
                                aims.Add(w);
                                break;
                            case CardEffect.CardAim.Allies:
                                if (w.Player != null)
                                {
                                    aims.Add(w);
                                }
                                break;
                            case CardEffect.CardAim.Ally:
                                break;
                            case CardEffect.CardAim.Any:
                                break;
                            case CardEffect.CardAim.Enemies:
                                if (w.Player == null)
                                {
                                    aims.Add(w);
                                }
                                break;
                            case CardEffect.CardAim.Enemy:
                                break;
                            case CardEffect.CardAim.You:
                                if (w.Player == PhotonNetwork.player)
                                {
                                    aims.Add(w);
                                }
                                break;
                        }	
					}
                    SelectAims(aims.ToArray());
                }
			}
        }
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
        
        ActiveCard = null;

		RPGCardGameManager.sInstance.RemoveCardFromHand(card.CardAsset, PhotonNetwork.player);

        if (card.CardAsset.DestroyAfterPlay)
        {
            card.SetState(CardVisual.CardState.Burned);
        }
        else
        {
			RPGCardGameManager.sInstance.AddCardToDrop(card.CardAsset, PhotonNetwork.player, LocalPlayerVisual.CardAnimationAim.Hand, false);
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
		SelectAims (aims);
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
		//cards
		cardEffects.Add (new AddCardEffect());
		cardEffects.Add (new TakeCardEffect());
		cardEffects.Add (new BurnEffect());
		cardEffects.Add (new DropEffect());

        //warriors
        cardEffects.Add(new DamageEffect());
        cardEffects.Add(new BlockEffect());
        cardEffects.Add(new AddModifierEffect());

		//additional
		cardEffects.Add (new ChooseEffect());
	}
}
