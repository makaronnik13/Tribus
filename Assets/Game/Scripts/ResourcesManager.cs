using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourcesManager : Singleton<ResourcesManager>
{
    public Action<GameResource, int> OnResourceValueChanged = (GameResource res, int value) => { };
    public List<Inkome> StartedReources = new List<Inkome>();

    private Dictionary<GameResource, int> ResourcesValues = new Dictionary<GameResource, int>();

    private void Start()
    {
        CardsPlayer.Instance.OnCardPlayed += PlayCard;
    }

    public void EndTurn()
    {
        foreach (Inkome gr in StartedReources)
        {
            SetResource(gr.resource, 0);
        }
    }

    public void StartTurn()
    {
			foreach(Inkome ink in GetIncomeForPlayer(GameLobby.Instance.CurrentPlayer))
            {
				AddResource(ink.resource, ink.value);
            }
    }

	public List<Inkome> GetIncomeForPlayer(Player player)
	{
		List<Inkome> inkome = new List<Inkome> ();
		inkome.Clear ();

		foreach (Inkome ink in StartedReources)
		{
			inkome.Add(ink);
		}


		foreach (Block cs in BlocksField.Instance.Blocks)
		{
			if(cs.Owner!=player)
			{
				continue;
			}
				
			foreach(Inkome ink in cs.CurrentIncome)
			{
					inkome.Add (ink);	
			}
		}

		return inkome;
	}

    public bool CardAvailability(Card cardAsset)
    {
        bool result = true;
		if(!cardAsset)
		{
			return false;
		}
        foreach (Inkome ink in cardAsset.Cost)
        {
            if (!ResourcesValues.ContainsKey(ink.resource))
            {
                ResourcesValues.Add(ink.resource, 0);
            }
            if (ink.value>ResourcesValues[ink.resource])
            {
                result = false;
            }
        }
        return result;
    }

    public void SetResource(GameResource gr, int v)
    {
        if (!ResourcesValues.ContainsKey(gr))
        {
            ResourcesValues.Add(gr, 0);
        }
        ResourcesValues[gr] = v;
        OnResourceValueChanged.Invoke(gr, v);
    }

    public void AddResource(GameResource gr, int v)
    {
        
        if (!ResourcesValues.ContainsKey(gr))
        {
            ResourcesValues.Add(gr, 0);
        }
        SetResource(gr, ResourcesValues[gr]+v);
    }

    public void PlayCard(Card c)
    {
        foreach (Inkome ink in c.Cost)
        {
            AddResource(ink.resource, -ink.value);
        }
    }
}
