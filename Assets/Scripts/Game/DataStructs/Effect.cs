using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "Effect")]
public class Effect : ScriptableObject 
{
	public Sprite effectImage;
	
	public enum EfectTrigerring
	{
		None,
		OnStartTurn,
		OnEndTurn,
		OnCardTaken,
		OnCardDroped,
		OnCardBurn,
		OnCardPlay
	}

	public int Time;

	public EfectTrigerring trigerOn = EfectTrigerring.None;

	public List<CardEffect> effects = new List<CardEffect>();
}
