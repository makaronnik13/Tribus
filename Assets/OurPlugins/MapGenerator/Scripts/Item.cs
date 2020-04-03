using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tribus/RPG/Item")]
public class Item : ScriptableObject 
{
	public string ItemName;
	public int Cost;
	public Sprite ItemSprite;
	public int level;
    public string ItemDescription;
}
