using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tribus/Chellenge/Variant")]
public class ChellengeVariant : ScriptableObject 
{
	public enum DiceSide
	{
		Fire,
		Water,
		Stone,
		Wind,
		Soul,
		Death,
		Animal,
		Plant
	}

	public bool DiceRoll;
	public List<DiceSide> NeedDices;
	public string text;

	public List<ChellengeVariantCondition> conditions;

	public CellengeState aimState;
}
