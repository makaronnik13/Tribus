using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Deck")]
public class Deck : ScriptableObject
{
	public List<Card> cards;
}
