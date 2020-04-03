
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Tribus/Battle/Battle")]
public class Battle: ScriptableObject
{
	public List<Warrior> Enemies;
    public float HardLvl;
}