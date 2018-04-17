
	using UnityEngine;
	using UnityEditor;

	//[InitializeOnLoad]
	public class StatesUpdater 	
	{
	public static StatesList AllStatesList
	{
		get
		{
			return Resources.Load<StatesList> ("AllStatesList");
		}
	}

		static StatesUpdater()
		{
		AllStatesList.States.Clear ();
			foreach (string s in AssetDatabase.FindAssets ("t:CellState")) 
			{
				AllStatesList.States.Add ((CellState)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(s), typeof(CellState)));
			}
		}
	}
