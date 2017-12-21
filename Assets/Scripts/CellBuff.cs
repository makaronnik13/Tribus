using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class CellBuff {

	public Condition condition;

	[DictionaryDrawerSettings]
	public Dictionary<GameResource, int> values;
}
