using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineModel {

	private static Dictionary<Skills, float> cooldowns = new Dictionary<Skills, float>()
	{
		{Skills.Flora, 5},
		{Skills.Fauna, 10},
		{Skills.Minerals, 20}
	};


	public enum Skills
	{
		None,
		Flora,
		Fauna,
		Minerals
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
		GrassBirg,
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
