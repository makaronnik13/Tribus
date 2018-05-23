using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tribus/Cards/Deck")]
public class Deck : ScriptableObject
{
	[SerializeField]
	public EditorDeckStruct DeckStruct;
}
