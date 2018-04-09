using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Combination  
{

	public CombineModel.Skills skill;
	public CellState ResultState;

	[Range(1,3)]
	public int skillLevel = 1;
}
