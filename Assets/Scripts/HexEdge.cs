using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexEdge : MonoBehaviour 
{
	public List<GameObject> EdgesVariants;

	public void SetEdge(int prev, int front, int next, bool none = false)
	{
		//Debug.Log (prev+"|"+front+"|"+next);

		if(none)
		{
			SetEdge (-1);
			return;
		}

		//Debug.Log (front);

		if(prev == 2 && front == 0 && next == 2)
		{
			SetEdge (7);
			return;
		}

		if(prev == 2 && front == 1 && next == 2)
		{
			SetEdge (1);
			return;
		}

		if(prev == 2 && front == 1 && (next == 1 || next == 0))
		{
			SetEdge (2);//5
			return;
		}

		if((prev == 1 || prev == 0) && front == 1 && next == 2)
		{
			SetEdge (3);//4
			return;
		}

		if(front == 0 && next == 2)
		{
			SetEdge (2);
			return;
		}

		if(front == 0 && prev == 2)
		{
			SetEdge (3);
			return;
		}

	


		if(front == 2)
		{
			SetEdge (0);
			return;
		}

		if(front == 0 || front == 1)
		{
			SetEdge (6);
			return;
		}

		SetEdge (2);
	}

	private void SetEdge(int v)
	{
		int i = 0;
		foreach(GameObject go in EdgesVariants)
		{
			go.SetActive (i==v);
			i++;
		}
	}
}
