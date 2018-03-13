using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Grids/Quest")]
public class Quest: ScriptableObject
{

	public string QuestName;

	[HideInInspector]
	[SerializeField]
	public float X, Y;

	public Quest[] nextQuest = new Quest[0];

	public void Drag(Vector2 p)
	{
		X = p.x;
		Y = p.y;
	}

	[MultiLineProperty]
	public string description;
}
