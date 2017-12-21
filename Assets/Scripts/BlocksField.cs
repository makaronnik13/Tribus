using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlocksField : Singleton<BlocksField> {
	public int width=4, height=4;

	public float cellSize = 2;

	public GameObject[] baseBlocks;

	public GameObject Highlighter;
	public GameObject BlockInfo;

	private List<ParticleSystem> highlighters = new List<ParticleSystem>();
	private List<BlockInfo> blocksInfos = new List<BlockInfo>();

	void Awake()
	{
		GenerateRandomTerrain ();
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.Tab))
		{
			ShowInfo (FindObjectsOfType<Block>().ToList());
		}
		if(Input.GetKeyUp(KeyCode.Tab))
		{
			ShowInfo (new List<Block>());
		}
	}

	public void GenerateRandomTerrain()
	{
		for(int i = 0; i<width;i++)
		{
			for(int j = 0; j<height; j++)
			{
				int v = Random.Range (0, baseBlocks.Length);
				Quaternion randomRotation = Quaternion.Euler (Vector3.up*90*Mathf.RoundToInt(Random.Range(0,3)));
				Instantiate (baseBlocks[v], transform.position+new Vector3((i-width/2)*cellSize,0,(j-height/2)*cellSize), randomRotation,transform.GetChild(0));
				highlighters.Add (Instantiate (Highlighter, transform.position+new Vector3((i-width/2)*cellSize,0,(j-height/2)*cellSize), Quaternion.identity,transform.GetChild(1)).GetComponentInChildren<ParticleSystem>());
				blocksInfos.Add (Instantiate (BlockInfo, transform.position+new Vector3((i-width/2)*cellSize,0,(j-height/2)*cellSize), Quaternion.identity,transform.GetChild(2)).GetComponent<BlockInfo>());
			}
		}

        foreach (Block b in FindObjectsOfType<Block>())
        {
            b.RecalculateInkome();
        }
	}

	public void HighLightFields(List<Block> blocks)
	{
		foreach(ParticleSystem ps in highlighters)
		{
			ps.Stop ();
		}

		foreach(Block b in blocks)
		{
			highlighters.Find (h=>Vector3.Distance(h.transform.position, b.transform.position)<1).Play();
		}
	}

	public void ShowInfo(List<Block> blocks)
	{
		foreach(BlockInfo bi in blocksInfos)
		{
			bi.Hide ();
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
        return FindObjectsOfType<Block>().Where(b => Vector3.Distance(b.transform.position, block.transform.position) < r * Mathf.Sqrt(2) * cellSize && b != block).ToList();
    }
}
