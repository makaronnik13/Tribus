using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class BlocksField : Photon.MonoBehaviour
{
    public static BlocksField Instance;
	public int Size = 4;
	public float CellSize = 2;
	private Dictionary<Vector2, Block> cells = new Dictionary<Vector2, Block> ();

	public List<CellState> baseStates;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GetComponent<PhotonView>().RPC("GenerateField", PhotonTargets.MasterClient, new object[0]);
    }

    public List<Block> Blocks
	{
		get
		{
			List<Block> blocks = new List<Block> ();
			foreach(KeyValuePair<Vector2, Block> pair in cells)
			{
				blocks.Add (pair.Value);
			}
			return blocks;
		}
	}

    public List<Block> GetBlocksInRadius(Block block, int r)
	{
		List<Block> blocks = new List<Block> ();
		if(block == null)
		{
			return blocks;
		}

		Vector2 blockPos = cells.FirstOrDefault (x => x.Value == block).Key;

		foreach(KeyValuePair<Vector2, Block> b in cells)
		{
			float deltaX = Mathf.Abs (b.Key.x - blockPos.x);
			float deltaY = Mathf.Abs (b.Key.y - blockPos.y); 
			if(deltaX<=r && deltaY<=r && b.Key.x - blockPos.x+b.Key.y - blockPos.y<=r && -b.Key.x + blockPos.x-b.Key.y + blockPos.y<=r)
			{
					blocks.Add(b.Value);
			}
		}
		return blocks;
    }
		
    [PunRPC]
	public void GenerateField()
	{
		cells.Clear ();
		foreach(Transform t in transform)
		{
			Destroy(t.gameObject);
		}


		foreach(KeyValuePair<Vector3, Vector2> v in RecalculateHexes())
		{
			GameObject newCell = PhotonNetwork.Instantiate ("BaseBlock", Vector3.zero, Quaternion.identity, 0);
			newCell.transform.SetParent (transform);
			newCell.transform.localScale = Vector3.one;
			newCell.transform.localRotation = Quaternion.Euler (Vector3.up*60*Mathf.RoundToInt(Random.Range(0,6)));
			newCell.transform.localPosition = v.Key;
			cells.Add (v.Value, newCell.GetComponent<Block>());
			//newCell.GetComponentInChildren<TextMeshProUGUI> ().text = v.Value.x + "/" + v.Value.y;
		}


		foreach(KeyValuePair<Vector2, Block> pair in cells)
		{
			int rand = Random.Range (0, 4);
			if (baseStates [rand]) {
				pair.Value.Biom = baseStates [rand].Biom;
			} else 
			{
				pair.Value.Biom = CombineModel.Biom.None;
			}

			pair.Value.State =  baseStates[rand];
		}

		foreach (EdgesController ec in GetComponentsInChildren<EdgesController>()) {
			ec.RecalculateEdges ();
		}

		foreach (Block b in FindObjectsOfType<Block>())
		{
			b.RecalculateInkome();
		}
	}

	public Dictionary<Vector3, Vector2> RecalculateHexes ()
	{
		List<Vector2> cellsCoordinates = new List<Vector2> ();
		Dictionary<Vector3, Vector2> result = new Dictionary<Vector3, Vector2> ();
		cellsCoordinates.Clear ();

		for (int i = -Size * 3; i < Size * 3; i++) {
			for (int j = -Size * 3; j < Size * 3; j++) {
				if (UnityEngine.Random.Range (0, 10) >= 0 && Mathf.Abs (i + j) < Size && Mathf.Abs (j) < Size && Mathf.Abs (i) < Size) {
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

	public Block GetCellFromSide(Block e, int i)
	{
		try
		{
			Vector2 cellPos = GetCellPos(e);
			switch(i)
			{
			case 0:
				return cells[new Vector2(cellPos.x-1, cellPos.y)];
			case 1:
				return cells[new Vector2(cellPos.x, cellPos.y-1)];
			case 2:
				return cells[new Vector2(cellPos.x+1, cellPos.y-1)];
			case 3:
				return cells[new Vector2(cellPos.x+1, cellPos.y)];
			case 4:
				return cells[new Vector2(cellPos.x, cellPos.y+1)];
			case 5:
				return cells[new Vector2(cellPos.x-1, cellPos.y+1)];
			}
		}
		catch
		{
			return null;
		}

		return null;
	}

	public Vector2 GetCellPos(Block e)
	{
		return cells.FirstOrDefault (x => x.Value == e).Key;
	}
}
