using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class CellBuff
{
	[System.Serializable]
	public enum CellBuffRadius
	{
		Adjusted,
		Radius,
		All
	}

	public enum BuffType
	{
		If,
		ForEvery
	}

	public enum CellCharacteristic
	{
		State,
		Biom,
		Resource
	}

	public CellBuffRadius radius;
	public BuffType buffType;
	public CellCharacteristic characteristic;

	public CellState[] states;
	public CombineModel.Biom[] bioms;
	public CombineModel.ResourceType[] resoureTypes;

	//public CheckType checkType;
	//public CellState state; 
	//public Inkome[] bonuses;
}

