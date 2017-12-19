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

	private static Dictionary<GameResource, string> resourcesImages = new Dictionary<GameResource, string>()
	{
		{GameResource.Wealth, "Sprites/GameResources/Wealth"},
		{GameResource.Nature, "Sprites/GameResources/Nature"},
		{GameResource.Danger, "Sprites/GameResources/Danger"},
		{GameResource.Technology, "Sprites/GameResources/Technology"},
		{GameResource.Food, "Sprites/GameResources/Food"}
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
		
	public enum GameResource
	{
		Wealth,
		Nature,
		Danger,
		Technology,
		Food
	}

	public enum ResourceType
	{
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

	public static Sprite GetResourceImage(GameResource res)
	{
		return Resources.Load<Sprite>(resourcesImages[res]);
	}
}
