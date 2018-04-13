using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class EdgesController : MonoBehaviour {

	public TMPro.TextMeshProUGUI text;
	private List<MeshRenderer> renderers = new List<MeshRenderer>();
	public CombineModel.Biom biom;
	public CombineModel.Biom Biom
	{
		get
		{
			return biom;
		}
		set
		{
			biom = value;
			foreach(MeshRenderer mr in renderers)
			{
				switch(biom)
				{
				case CombineModel.Biom.Forest:
					mr.material.color = Color.green;
					break;
				case CombineModel.Biom.Mountains:
					mr.material.color = Color.gray;
					break;
				case CombineModel.Biom.Water:
					mr.material.color = Color.blue;
					break;
				}
			}
		}
	}

	void Awake()
	{
		foreach(MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
		{
			renderers.Add (mr);
		}
	}
		
	[Button("Set")]
	public void RecalculateEdges()
	{
		FieldCreator fc = GetComponentInParent<FieldCreator> ();
		GetComponent<MeshRenderer> ().enabled = (biom != CombineModel.Biom.None);

		int i = 6;
		foreach(HexEdge he in GetComponentsInChildren<HexEdge>())
		{

			int h = i - 1;
			int j = i + 1;
			if(i>5)
			{
				i -= 6;
			}
			if(h>5)
			{
				h -=  6;
			}
			if(j>5)
			{
				j -= 6;
			}
				
			EdgesController b1 = fc.GetCellFromSide(this, h);
			EdgesController b2 = fc.GetCellFromSide(this, i);
			EdgesController b3 = fc.GetCellFromSide(this, j);

			//Debug.Log (h+"|"+i+"|"+j);
			//Debug.Log (b1+"|"+b2+"|"+b3);

			int t1 = GetSideType(b1);
			int t2 = GetSideType(b2);
			int t3 = GetSideType(b3);

			he.SetEdge (t1, t2, t3 , biom == CombineModel.Biom.None);
			i++;
		}
	}

	private int GetSideType(EdgesController c)
	{
		if(!c)
		{
			return 0;
		}

		if(c.Biom == CombineModel.Biom.None)
		{
			return 0;
		}
		if(c.Biom == Biom)
		{
			return 2;
		}

		return 1;
	}
}
