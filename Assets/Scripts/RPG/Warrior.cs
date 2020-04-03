using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(menuName = "Tribus/Global/Warrior")]
public class Warrior : ScriptableObject {

	public Sprite sprite;
	public float initiative;
	public int hp;
	public int damage;
	public GameObject visual;
	public string WarriorName;

    public bool Enemy = true;


    [HideIf("Enemy")]
    public Deck startingDeck;

}
