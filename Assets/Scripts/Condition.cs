using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[System.Serializable]
public class Condition
{
	[System.Serializable]
	public enum CellBuffRadius
	{
		Adjusted,
		Radius,
		All,
        This
	}

	public enum BuffType
	{
		ForEvery,
        More,
        Less,
        Equal
    }

	public enum CellCharacteristic
	{
		State,
		Biom,
		Resource
	}

	public CellBuffRadius radius;
	public BuffType buffType;

    [ShowIf("IsForeach")]
    public int maxStack = 1000;

    [ShowIf("TypeValue")]
    public int value;
	public CellCharacteristic characteristic;

    [ShowIf("ShowDifferentToggle")]
    public bool onlyDifferent = false;

    [ShowIf("UseState")]
    public CellState[] states;
    [ShowIf("UseBiom")]
    public CombineModel.Biom[] bioms;
	[ShowIf("UseResource")]
	public GameResource[] resources;

    [ShowInInspector, SerializeField]
    public List<Inkome>rewardResources;

    private bool ShowDifferentToggle()
    {
        return characteristic != CellCharacteristic.Resource;
    }

    private bool UseState()
    {
        return characteristic == CellCharacteristic.State;
    }
    private bool UseBiom()
    {
        return characteristic == CellCharacteristic.Biom;
    }
	private bool UseResource()
	{
		return characteristic == CellCharacteristic.Resource;
	}
    private bool TypeValue()
    {
        return buffType != BuffType.ForEvery;
    }
    private bool IsForeach()
    {
        return buffType == BuffType.ForEvery;
    }


    public int ChechCondition(Block b)
    {
        List<Block> blocks = new List<Block>();

        if (radius == CellBuffRadius.Adjusted)
        {
            blocks = BlocksField.Instance.GetBlocksInRadius(b, 1);
        }

        if (radius == CellBuffRadius.Radius)
        {
			Debug.Log (b.Radius);
            blocks = BlocksField.Instance.GetBlocksInRadius(b, b.Radius);
        }

        if (radius == CellBuffRadius.All)
        {
            blocks = BlocksField.Instance.GetBlocksInRadius(b, int.MaxValue);
        }


        if (radius == CellBuffRadius.This)
        {
            blocks = new List<Block>() { b };
        }

        float number = 0;

        List<CombineModel.Biom> checkedBioms = new List<CombineModel.Biom>();
        List<CellState> checkedStates = new List<CellState>();
        List<CombineModel.ResourceType> chackedTypes = new List<CombineModel.ResourceType>();

        foreach (Block block in blocks)
        {
			Debug.Log (block.State);
            switch (characteristic)
            {
                case CellCharacteristic.Biom:
                    if (bioms.ToList().Contains(block.State.Biom))
                    {
                        if (!onlyDifferent)
                        {
                            number++;
                        }
                        else
                        {
                            if (!checkedBioms.Contains(block.State.Biom))
                            {
                                number++;
                                checkedBioms.Add(block.State.Biom);
                            }
                        }
                    }
                    break;
                case CellCharacteristic.Resource:
                    List<Inkome> inkomes = block.CurrentIncome.Where(income => resources.Contains(income.resource)).ToList();
                    foreach (Inkome inc in inkomes)
                    {
                        number += inc.value;
                    }
                    break;
                case CellCharacteristic.State:
                    if (states.ToList().Contains(block.State))
                    {
                        if (!onlyDifferent)
                        {
                            number++;
                        }
                        else
                        {
                            if (!checkedStates.Contains(block.State))
                            {
                                number++;
                                checkedStates.Add(block.State);
                            }
                        }
                    }
                    break;
               
            }
        }


        if (buffType == BuffType.Equal)
        {
            if(number == value)
            {
                number = 1;
            }
            else
            {
                number = 0;     
            }
        }

        if (buffType == BuffType.Less)
        {
			Debug.Log (number);

            if (number < value)
            {
                number = 1;
            }
            else
            {
                number = 0;
            }
        }

        if (buffType == BuffType.More)
        {
            if (number > value)
            {
                number = 1;
            }
            else
            {
                number = 0;
            }
        }

		Debug.Log (number);
        return Mathf.RoundToInt(number);
    }
}

