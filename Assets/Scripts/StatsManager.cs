using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatsManager : Singleton<StatsManager> {

	public GameResource[] resources;
	public Action OnStatChanged;
    public float incomeRate = 3;
    public List<Inkome> currentResources = new List<Inkome>();
	public List<Inkome> incomes 
	{
		get
		{
			List<Inkome> result = new List<Inkome> ();

			foreach(Block b in FindObjectsOfType<Block>())
			{
				foreach(Inkome inc in b.CurrentIncome)
				{
					
					if (result.Find (inca => inca.resource == inc.resource) == null) {
						Inkome newInc = new Inkome ();
						newInc.resource = inc.resource;
						newInc.value = inc.value;
						result.Add (newInc);
					} else {
						result.Find (r => r.resource == inc.resource).value += inc.value;
					}
				}

                
			}

			return result;
		}	
	}

	void Start()
	{
		//OnStatChanged.Invoke ();
		//InvokeRepeating ("ApplyIncome", 1, incomeRate);
	}

	public void UpdateStats()
	{
		List<Inkome> newStats = new List<Inkome> ();

		foreach (Block b in FindObjectsOfType<Block>()) {
			foreach (Inkome inc in b.CurrentIncome) 
			{
				if(inc.resource.showInPanel)
				{
					if (newStats.Find (inca => inca.resource == inc.resource) == null) {
						Inkome newInc = new Inkome ();
						newInc.resource = inc.resource;
						newInc.value = inc.value;
						newStats.Add (newInc);
					} else {
						newStats.Find (r => r.resource == inc.resource).value += inc.value;
					}
				}
			}
		}
		OnStatChanged.Invoke ();
	}

	public void ApplyIncome()
	{
		foreach(Block b in FindObjectsOfType<Block>())
		{
			foreach(Inkome inc in b.CurrentIncome)
			{
                if (inc.resource.incoming)
                {
                    currentResources.Find(r => r.resource == inc.resource).value += inc.value;
                }
				
			}

           BlocksField.Instance.Emmit();
        }

		if (OnStatChanged != null) {
			OnStatChanged.Invoke ();
		}
	}
}
