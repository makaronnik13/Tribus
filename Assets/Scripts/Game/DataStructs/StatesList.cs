using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StatesList")]
public class StatesList : ScriptableObject 
{
	public List<CellState> States;
}
