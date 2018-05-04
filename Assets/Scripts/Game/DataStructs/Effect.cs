using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "Effect")]
public class Effect : ScriptableObject 
{
	public Sprite effectImage;

	public float addHpEveryTurn = 0;
	public bool multiplyOnLength = false;
}
