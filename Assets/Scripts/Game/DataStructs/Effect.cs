using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "Tribus/Battle/Effect")]
public class Effect : ScriptableObject 
{
	public Sprite effectImage;

	public float addHpEveryTurn = 0;
	public bool multiplyOnLength = false;

    public int extraBlock = 0;
    public int extraDamage = 0;
}
