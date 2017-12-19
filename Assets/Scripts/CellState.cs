using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Grids/CellState")]
public class CellState: ScriptableObject
{

	#if UNITY_EDITOR
	public float X, Y;

	public void Drag(Vector2 p)
	{
		X = p.x;
		Y = p.y;
	}

	#endif

	public GameObject prefab;
	public Inkome[] income;

	public Combination[] Combinations
	{
		get
		{
			if(combinations == null)
			{
				combinations = new Combination[0];
			}
			return combinations;
		}
		set
		{
			combinations = value;
		}
	}

	[SerializeField]
	private Combination[] combinations;

	public CellBuff[] buffs; 

	public bool HasCombination(CombineModel.Skills skill)
	{
		foreach(Combination comb in Combinations)
		{
			if(comb.skill == skill)
			{
				return true;
			}
		}
		return false;
	}

	public CellState CombinationResult(CombineModel.Skills skill)
	{
		foreach(Combination comb in Combinations)
		{
			if(comb.skill == skill)
			{
				return comb.ResultState;
			}
		}
		return null;
	}

	public void AddCombination()
	{
		Combination c = new Combination ();
		List<Combination> comb = Combinations.ToList ();
		comb.Add(c);
		Combinations = comb.ToArray ();
	}


	public void RemoveCombination(int i)
	{
		List<Combination> comb = Combinations.ToList ();
		comb.RemoveAt (i);
		Combinations = comb.ToArray ();
	}

	public void AddIncome()
	{
		List<Inkome> ink = income.ToList ();
		ink.Add(new Inkome());

	
		income = ink.ToArray ();

	}
		
	public void RemoveIncome(int i)
	{
		List<Inkome> incomes = income.ToList ();
		incomes.RemoveAt (i);
		income = incomes.ToArray ();
	}

	public void AddBuff()
	{
		List<CellBuff> ink = buffs.ToList ();
		ink.Add(new CellBuff());
		buffs = ink.ToArray ();
	}

	public void RemoveBuff(int i)
	{
		List<CellBuff> incomes = buffs.ToList ();
		incomes.RemoveAt (i);
		buffs = incomes.ToArray ();
	}

}
