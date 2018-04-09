using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusVisualizer : Singleton<RadiusVisualizer> {

	private Projector projector;
	private Block showingBlock;

	// Use this for initialization
	void Start () {
		projector = GetComponentInChildren<Projector> ();	
	}
	
	public void ShowRadius(Block block)
	{
		if(showingBlock!=block)
		{
			if (!block) {
				projector.enabled = false;
			} else 
			{
				projector.enabled = true;
				transform.position = block.transform.position;
				projector.orthographicSize = 1 + 2 * block.Radius;
			}
			showingBlock = block;
		}
	}
}
