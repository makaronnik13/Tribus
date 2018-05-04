using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
	[FoldoutGroup("Card info")]
    public string CardName;
	[FoldoutGroup("Card info")]
    public string CardDescription;
	[FoldoutGroup("Card info")]
	public Sprite cardSprite;
	public List<Inkome> Cost = new List<Inkome>();
	[FoldoutGroup("Card info")]
	public bool WinCard = false;
	[FoldoutGroup("Card info")]
	public bool DestroyAfterPlay;

	public List<CardEffect> CardEffects = new List<CardEffect>();

	public int MoneyCost;
	public int Level;
}
