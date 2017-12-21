using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Grids/CellState")]
public class CellState: ScriptableObject
{

    public string StateName;
	public CombineModel.ResourceType type;
    public CombineModel.Biom Biom;

	#if UNITY_EDITOR
    [HideInInspector]
	public float X, Y;

	public void Drag(Vector2 p)
	{
		X = p.x;
		Y = p.y;
	}

#endif

    public enum RadiusType
    {
        Simple,
        FromStat
    }
    public RadiusType radiusType = RadiusType.Simple;

    [ShowIf("RadiusSimple")]
    public int Radius = 1;
    [ShowIf("RadiusFromStat")]
    public GameResource radiusResource;

    private bool RadiusSimple()
    {
        return radiusType == RadiusType.Simple;
    }

    private bool RadiusFromStat()
    {
        return radiusType == RadiusType.FromStat;
    }

    [MultiLineProperty]
	public string description;

    [AssetsOnly, InlineEditor(InlineEditorModes.LargePreview)]
	public GameObject prefab;

    [AssetsOnly, InlineEditor(InlineEditorModes.LargePreview)]
    public Sprite Sprite;

    

    public List<CombineModel.ResourceType> types;


    [TabGroup("incomes"), InlineProperty]
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
    [TabGroup("combinations")]
	private Combination[] combinations;

    [TabGroup("conditions")]
    public Condition[] conditions; 

	[TabGroup("auras")]
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
		List<Condition> ink = conditions.ToList ();
		ink.Add(new Condition());
		conditions = ink.ToArray ();
	}

	public void RemoveBuff(int i)
	{
		List<Condition> incomes = conditions.ToList ();
		incomes.RemoveAt (i);
		conditions = incomes.ToArray ();
	}

}
