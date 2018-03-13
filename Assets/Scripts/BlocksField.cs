using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlocksField : Singleton<BlocksField> {
	public int size = 4;
	public float cellSize = 2;

	public GameObject[] baseBlocks;

	public GameObject Highlighter;
	public GameObject BlockInfo;

	private Dictionary<Vector3, Projector> highlighters = new Dictionary<Vector3,Projector>();
	private List<BlockInfo> blocksInfos = new List<BlockInfo>();

	private bool infoShowing;
	public bool InfoShowing
	{
		set
		{
			if(infoShowing!=value)
			{
				infoShowing = value;
				if (infoShowing) {
					ShowInfo (FindObjectsOfType<Block> ().ToList ());
				} else {
					ShowInfo (new List<Block>());
				}
			}
		}
		get
		{
			return infoShowing;
		}
	}

	void Awake()
	{
		GenerateRandomTerrain ();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			InfoShowing = true;
		}
		if(Input.GetKeyUp(KeyCode.Tab))
		{
			InfoShowing = false;
		}
	}

	public void GenerateRandomTerrain()
	{
		GetComponent<DiamondSquareTest> ().size = size;
		GetComponent<DiamondSquareTest> ().roughness = 40;
		Dictionary<Vector2, int> dict = GetComponent<DiamondSquareTest> ().GetBiomes ();

		for(int i = 0; i<Mathf.Pow(2,size)+1;i++)
		{
			for(int j = 0; j<Mathf.Pow(2,size)+1; j++)
			{
				GameObject block = baseBlocks[dict[new Vector2(i,j)]];
				float width = Mathf.Pow (2, size) + 1;
				Quaternion randomRotation = Quaternion.Euler (Vector3.up*90*Mathf.RoundToInt(Random.Range(0,3)));
				GameObject nb = Instantiate (block, transform.position+new Vector3((i-width/2)*cellSize,0,(j-width/2)*cellSize), randomRotation,transform.GetChild(0));
				highlighters.Add (nb.transform.position, Instantiate (Highlighter, transform.position+new Vector3((i-width/2)*cellSize,0,(j-width/2)*cellSize), Quaternion.identity,transform.GetChild(1)).GetComponentInChildren<Projector>());
				blocksInfos.Add (Instantiate (BlockInfo, transform.position+new Vector3((i-width/2)*cellSize,0,(j-width/2)*cellSize), Quaternion.identity,transform.GetChild(2)).GetComponent<BlockInfo>());
			}
		}

        foreach (Block b in FindObjectsOfType<Block>())
        {
            b.RecalculateInkome();
        }

		HighLightFields (new List<Block>(){});

		foreach(Block block in FindObjectsOfType<Block>())
		{
			List<Block> blocks = GetBlocksInRadius (block, 1);
			blocks.Add (block);

			foreach(Block neighbour in blocks)
			{
				block.OnBiomChanged+=(CombineModel.Biom biom, Block n)=>
				{
					int i = -1;

					if(n == block)
					{
						i = 0;
					}

					if(n.transform.position.x<block.transform.position.x && n.transform.position.y == block.transform.position.y)
					{
						i = 1;
					}

					if(n.transform.position.x==block.transform.position.x && n.transform.position.y > block.transform.position.y)
					{
						i = 2;
					}

					if(n.transform.position.x>block.transform.position.x && n.transform.position.y == block.transform.position.y)
					{
						i = 3;
					}

					if(n.transform.position.x==block.transform.position.x && n.transform.position.y < block.transform.position.y)
					{
						i = 4;
					}

					block.RecalculateMesh(biom, i);
				};
			}
		}
	}

	public void HighLightFields(List<Block> blocks)
	{
		foreach(KeyValuePair<Vector3, Projector> ps in highlighters)
		{
			ps.Value.enabled = false;
		}

		foreach(Block b in blocks)
		{
			highlighters[b.transform.position].enabled = true;
		}
	}

	public void ShowInfo(List<Block> blocks)
	{
		if(!BlocksField.Instance.InfoShowing)
		{
		foreach(BlockInfo bi in blocksInfos)
		{
			bi.Hide ();
		}
		}
		foreach(Block b in blocks)
		{
			blocksInfos.Find (h=>Vector3.Distance(h.transform.position, b.transform.position)<1).Show(b);
		}
	}

    public void Emmit()
    {
        foreach (Block b in FindObjectsOfType<Block>())
        {
            blocksInfos.Find(h => Vector3.Distance(h.transform.position, b.transform.position) < 1).Emmit(b);
        }
    }

    public List<Block> GetBlocksInRadius(Block block, int r)
    {
        return FindObjectsOfType<Block>().Where(b => Vector3.Distance(b.transform.position, block.transform.position) <= r * Mathf.Sqrt(2) * cellSize+0.001f && b != block).ToList();
    }
}
