using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class DiamondSquareTest : MonoBehaviour {

	public Dictionary<Vector2, int> biomes = new Dictionary<Vector2, int>(){
		{new Vector2(0.8f,0f), 0},
		{new Vector2(0f,0.8f), 1},
		{new Vector2(0.5f,0.5f), 2},
		{new Vector2(0f,0.7f), 2},
		{new Vector2(0.6f,0f), 1}
	};

	[Range(1,10)]
	public int size = 3;

	[Range(-0.01f,500f)]
	public float roughness = 1;



	public Dictionary<Vector2, int> GetBiomes()
	{
		float[][] heightsValues = DiamondSqare.DiamondSquareGrid (size, (int)Random.Range(0,int.MaxValue), 0,  100, roughness);
		float[][] waterValues = DiamondSqare.DiamondSquareGrid (size, (int)Random.Range(0,int.MaxValue), 0,  100, roughness);

		Dictionary<Vector2, int> result = new Dictionary<Vector2, int> ();

		for (int i = 0; i < heightsValues.Length; i++) {
			for (int j = 0; j < heightsValues[0].Length; j++) 
			{
				Vector2 state = new Vector2( heightsValues [i] [j], waterValues[i][j]);

				KeyValuePair<Vector2, int> min = new KeyValuePair<Vector2, int> (Vector2.one*10000, 0);

				foreach(KeyValuePair<Vector2, int> kvp in biomes)
				{
					if(Vector2.Distance(kvp.Key, state)<Vector2.Distance(min.Key, state))
					{
						min = kvp;
					}	
				}
							
				result.Add (new Vector2(i, j), min.Value);
			}
		}
			

		return result;
	}
}
