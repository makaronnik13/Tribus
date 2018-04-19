using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class DefaultResourcesManager
{
	private static string AtlasPath = "Assets/DefaultResources/PortraitsAtlas";
	private static string NamesAssetPath = "Assets/DefaultResources/PlayerNames";
	private static string ColorsAssetPath = "Assets/DefaultResources/PlayerColors";
	private static string DeckAssetPath = "Assets/DefaultResources/StartingDeck";
	private static string AllCardsAssetPath = "Assets/DefaultResources/AllCards";
    private static string StatesPath = "Assets/DefaultResources/AllStatesList";


	private static Card[] allCards;
	public static Card[] AllCards
	{
		get
		{
			if (allCards == null) 
			{
				allCards = Resources.Load<Deck> (AllCardsAssetPath).DeckStruct.Cards.ToArray();
			}
			return allCards;
		}
	}

	private static DeckStruct startingDeck;
	public static DeckStruct StartingDeck
	{
		get
		{
			if (startingDeck == null) 
			{
                List<string> cards = new List<string>();
                foreach (Card c in Resources.Load<Deck>(DeckAssetPath).DeckStruct.Cards)
                {
                    cards.Add(c.name);
                }
                startingDeck = new DeckStruct(Resources.Load<Deck>(DeckAssetPath).DeckStruct.DeckName, cards);
            }

			return startingDeck;
		}
	}

	private static string[] names = new string[0];
	public static string[] Names
	{
		get
		{
			if(names.Length == 0)
			{
				names = Resources.Load<StringsCollection> (NamesAssetPath).Names.ToArray();
			}
			return names;
		}
	}

	private static Color[] colors = new Color[0];
	public static Color[] Colors
	{
		get
		{
			if(colors.Length == 0)
			{
				colors = Resources.Load<ColorsCollection> (ColorsAssetPath).Colors.ToArray();
			}
			return colors;
		}
	}
		
	private static Sprite[] avatars = new Sprite[0];
	public static Sprite[] Avatars
	{
		get
		{
			if(avatars.Length == 0)
			{
				avatars = Resources.LoadAll<Sprite>(AtlasPath);
			}
			return avatars;
		}
	}

	public static int GetRandomAvatar()
	{
		return UnityEngine.Random.Range(0, Avatars.Length-1);
	}

	public static string GetRandomName()
	{
		return Names[UnityEngine.Random.Range(0, Names.Length-1)];
	}

	public static Color GetRandomColor()
	{
		return Colors[UnityEngine.Random.Range(0, Colors.Length-1)];
	}

    public static StatesList AllStatesList
    {
        get
        {
            return Resources.Load<StatesList>(StatesPath);
        }
    }

    public static List<CellState> PreviousStates(CellState state)
    {
        List<CellState> result = new List<CellState>();

        foreach (CellState cs in AllStatesList.States)
        {
            Combination comb = cs.Combinations.FirstOrDefault(c => c.ResultState == state);
            if (comb != null)
            {
                result.Add(cs);
            }
        }

        return result;
    }

	public static Card GetCardById(string id)
	{

         Card card = AllCards.ToList().FirstOrDefault(c => c.name == id);
        if (card == null)
        {
            throw new Exception("Card "+ id+ " does not contains in all cards!");
        }
         return card;
	}
		
}
