using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StatesInfo : Singleton<StatesInfo> {


	public static StatesList AllStatesList
	{
		get
		{
			return Resources.Load<StatesList> ("AllStatesList");
		}
	}

	public List<CellState> previousStates(CellState state)
	{
		List<CellState> result = new List<CellState> ();

		foreach(CellState cs in AllStatesList.States)
		{
			Combination comb = cs.Combinations.FirstOrDefault (c => c.ResultState == state);
			if(comb!=null)
			{
				result.Add (cs);		
			}
		}

		return result;
	}
}
