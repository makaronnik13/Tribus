using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

public class FieldCreator : MonoBehaviour {
	
	public float CellSize;
	public int xSize = 10;
	public GameObject cellPrefab;

	void Start()
	{
		GenerateField ();
	}

	[Button("Generate")]
	public void GenerateField()
	{
		foreach(Transform t in transform)
		{
            Lean.Pool.LeanPool.Despawn(t.gameObject);
		}


		foreach(KeyValuePair<Vector3, Vector2> v in RecalculateHexes())
		{
			GameObject newCell = Lean.Pool.LeanPool.Spawn(cellPrefab);
			newCell.transform.SetParent (transform);
			newCell.transform.localScale = Vector3.one;
			newCell.transform.localRotation = Quaternion.identity;
			newCell.transform.localPosition = v.Key;
		}
	}

	public Dictionary<Vector3, Vector2> RecalculateHexes ()
	{
		List<Vector2> cellsCoordinates = new List<Vector2> ();
		Dictionary<Vector3, Vector2> result = new Dictionary<Vector3, Vector2> ();
		cellsCoordinates.Clear ();

		for (int i = -xSize * 3; i < xSize * 3; i++) {
			for (int j = -xSize * 3; j < xSize * 3; j++) {
				if (UnityEngine.Random.Range (0, 10) >= 0 && Mathf.Abs (i + j) < xSize && Mathf.Abs (j) < xSize && Mathf.Abs (i) < xSize) {
					cellsCoordinates.Add (new Vector2 (i, j));
				}
			}
		}

		foreach (Vector2 c in cellsCoordinates) {
			Vector2 cell2DCoord = CellCoordToWorld (c);
			Vector3 cellPosition = RotatePointAroundPivot (new Vector3 (cell2DCoord.x, transform.position.y, cell2DCoord.y), transform.position, transform.rotation.eulerAngles);
			result.Add (cellPosition, c);
		}

		return result;
	}

	private Vector2 CellCoordToWorld (Vector2 cellCoord)
	{
		float x =  CellSize * (float)Mathf.Sqrt (3) * (cellCoord.x + cellCoord.y / 2);
		float y =  CellSize * 3 / 2 * -cellCoord.y;
		Vector3 pos = transform.position + new Vector3 (x, 0, y) * 0.6f;
		return new Vector2 (pos.x, pos.z);
	}

	private Vector3 RotatePointAroundPivot (Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler (angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it
	}


}
