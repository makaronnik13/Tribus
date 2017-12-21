using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

[System.Serializable]
public class Condition
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
		Type,
		Resource
	}

	public CellBuffRadius radius;
	public BuffType buffType;
	public CellCharacteristic characteristic;

    [ShowIf("UseState")]
    public CellState[] states;
    [ShowIf("UseBiom")]
    public CombineModel.Biom[] bioms;
    [ShowIf("UseType")]
    public CombineModel.ResourceType[] types;
	[ShowIf("UseResource")]
	public GameResource[] resources;

    private bool UseState()
    {
        return characteristic == CellCharacteristic.State;
    }
    private bool UseBiom()
    {
        return characteristic == CellCharacteristic.Biom;
    }
    private bool UseType()
    {
		return characteristic == CellCharacteristic.Type;
    }
	private bool UseResource()
	{
		return characteristic == CellCharacteristic.Resource;
	}
}

