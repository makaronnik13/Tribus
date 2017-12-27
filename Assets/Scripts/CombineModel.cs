using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineModel {

	private static Dictionary<Skills, float> cooldowns = new Dictionary<Skills, float>()
	{
		{Skills.Flora, 3},
		{Skills.Fauna, 5},
		{Skills.Minerals, 10},
		{Skills.Super, 15}
	};


	public enum Skills
	{
		None,
		Flora,
		Fauna,
		Minerals,
		Super
	}
		
	public enum Biom
	{
		Forest,
		Desert,
		Water,
		Bog,
		Mountains
	}
		

	public enum ResourceType
	{
        None,
		Tree,
		SmallPlant,
		Fish,
		GrassBird,
		PredatorBird,
		GrassAnimal,
		PredatorAnimal,
		BigAnimal,
		Minerals,
		Liquids
	}

	public static float SkillCd(Skills skill)
	{
		return cooldowns [skill];
	}

}
