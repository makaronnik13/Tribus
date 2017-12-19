using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

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

    [ShowIf("UseState")]
    public CellState[] states;
    [ShowIf("UseBiom")]
    public CombineModel.Biom[] bioms;
    [ShowIf("UseResourceType")]
    public CombineModel.ResourceType[] resoureTypes;

    private bool UseState()
    {
        return characteristic == CellCharacteristic.State;
    }
    private bool UseBiom()
    {
        return characteristic == CellCharacteristic.Biom;
    }
    private bool UseResourceType()
    {
        return characteristic == CellCharacteristic.Resource;
    }
    //public CheckType checkType;
    //public CellState state; 
    //public Inkome[] bonuses;
}

