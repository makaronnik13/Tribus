using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
	public enum CardAimType
	{
		None,
		Cell,
		Player
	}
		
    public string CardName;
    public string CardDescription;
	public CardAimType aimType = CardAimType.None;
	public Sprite cardSprite;
    public List<Inkome> Cost = new List<Inkome>();

	[ShowIf("ShowCellAim")]
	public CombineModel.Skills skill;
	[ShowIf("ShowCellAim")]
	public int skillLevel;

	public enum CellOwnership
	{
		Every,
		Neutral,
		Player,
		Oponent
	}

	private bool ShowCellAim()
	{
		return aimType == CardAimType.Cell;
	}
}
