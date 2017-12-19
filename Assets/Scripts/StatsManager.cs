using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatsManager : Singleton<StatsManager> {

	public Action OnStatChanged;

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
		foreach (CombineModel.GameResource r in Enum.GetValues(typeof(CombineModel.GameResource)))
		{
			Inkome inc = new Inkome();
			inc.value = 0;
			inc.resource = r;
			currentResources.Add (inc);
		}
		OnStatChanged.Invoke ();
		InvokeRepeating ("ApplyIncome", 1,5 );
	}

	public void ApplyIncome()
	{
		foreach(Block b in FindObjectsOfType<Block>())
		{
			foreach(Inkome inc in b.CurrentIncome)
			{
				currentResources.Find (r=>r.resource == inc.resource).value+=inc.value;
			}
		}

		if (OnStatChanged != null) {
			OnStatChanged.Invoke ();
		}
	}
}
