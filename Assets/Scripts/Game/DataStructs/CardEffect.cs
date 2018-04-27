using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]

public class CardEffect 
{

	public enum CardAim
	{
        None,
		You,
		Any,
		All,
		Enemy,
		Enemies,
        Ally,
        Allies
	}

	public enum CardsAimType
	{
		Pile,
		Hand,
		Drop,
		All
	}

    public enum CardsActionType
    {
        AddCards,
        DropCards,
        BurnCards,
        TakeCards
    }

    public enum WariorActionType
    {
        None,
        Damage,
        Block,
        AddEffect
    }

    public CardAim cardAim;

    [ShowIf("HasAim")]
    public WariorActionType warriorActionType = WariorActionType.Damage;
    private bool HasAim
    {
        get
        {
            return cardAim != CardAim.None;
        }
    }

    [ShowIf("HasCardAim")]
    public CardsActionType playerActionType = CardsActionType.TakeCards;
    private bool HasCardAim
    {
        get
        {
            return (cardAim == CardAim.Allies || cardAim == CardAim.Ally || cardAim == CardAim.You) && warriorActionType== WariorActionType.None;
        }
    }

    [ShowIf("HasCardSubAim")]
    public CardsAimType cardsAimType = CardsAimType.Hand;
    private bool HasCardSubAim
    {
        get
        {
            return HasCardAim && (playerActionType != CardsActionType.TakeCards) ;
        }
    }

    [ShowIf("ShowCardsList")]
    public List<Card> Cards = new List<Card>();
    private bool ShowCardsList
    {
        get
        {
            return (HasCardAim && (playerActionType == CardsActionType.AddCards))|| cardAim == CardAim.None;
        }
    }

    [ShowIf("ShowNumberOfCards")]
    public int NumberOfCards = 1;
    private bool ShowNumberOfCards
    {
        get
        {
            return HasCardAim && (playerActionType != CardsActionType.AddCards);
        }
    }

    [ShowIf("ShowNumberOfChosenCards")]
    public int NumberOfChosenCards = 0;
    private bool ShowNumberOfChosenCards
    {
        get
        {
            return HasCardSubAim;
        }
    }

    [ShowIf("ShowValue")]
    public int Value = 1;
    private bool ShowValue
    {
        get
        {
            return warriorActionType == WariorActionType.Block || warriorActionType == WariorActionType.Damage;
        }
    }

    [ShowIf("ShowEffect")]
    public Effect addingEffect;
    [ShowIf("ShowEffect")]
    public float duration;

    private bool ShowEffect
    {
        get
        {
            return warriorActionType == WariorActionType.AddEffect;
        }
    }
}
