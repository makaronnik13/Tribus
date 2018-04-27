using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Warrior")]
public class Warrior : ScriptableObject {

	public Sprite sprite;
	public float initiative;
	public int hp;
	public int damage;
	public GameObject visual;
	public string WarriorName;
}
