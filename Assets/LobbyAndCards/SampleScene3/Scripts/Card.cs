using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
