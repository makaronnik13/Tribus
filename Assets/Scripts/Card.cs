using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grids/Card")]
public class Card : ScriptableObject {
	public string CardName;
	public string description;

	public List<Inkome> price;

	public enum CardAimType
	{
		Cell,
		Player,
		Simple
	}

	public CardAimType aimType;

	//cell
	//public TypesMask 
	//public OwnersMask
	public bool destroyCell = false;
	public bool setBiom = false;
	public CombineModel.Biom biomType;
	public CombineModel.Skills skill;
	public int skillLevel;
}
