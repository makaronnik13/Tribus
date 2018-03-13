using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineModel {

	private static Dictionary<Skills, float> cooldowns = new Dictionary<Skills, float>()
	{
		{Skills.Flora, 1},
		{Skills.Fauna, 1},
		{Skills.Minerals, 1},
		{Skills.Super, 1}
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
