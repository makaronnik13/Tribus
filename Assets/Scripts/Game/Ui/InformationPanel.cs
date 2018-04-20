using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InformationPanel : Singleton<InformationPanel>
{
    public Image icon;
	public TMPro.TextMeshProUGUI blockName;
    public Transform incomeContent;
    public TMPro.TextMeshProUGUI symbiosys;
    public GameObject incomePrefab;
	private Block showingBlock;

	// Use this for initialization
	void Start () {
        transform.GetChild(0).gameObject.SetActive(false);
        symbiosys.gameObject.SetActive(false);
    }
	
    public void ShowInfo(Block block)
    {
		if (block == null || block.State == null)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            symbiosys.gameObject.SetActive(false);
            showingBlock = block;
        }
		else if(showingBlock!=block)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            symbiosys.gameObject.SetActive(true);
            foreach (Transform t in incomeContent)
            {
                Lean.Pool.LeanPool.Despawn(t.gameObject);
            }
            blockName.text = block.State.StateName;
            icon.sprite = block.State.Sprite;

            foreach (Inkome inc in block.State.income)
            {
                GameObject newRaw = Instantiate(incomePrefab, incomeContent);
                newRaw.transform.localScale = Vector3.one;
                newRaw.GetComponentInChildren<Image>().sprite = inc.resource.sprite;
				newRaw.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = inc.value + "";
            }

            showingBlock = block;
            symbiosys.text = ApplyTags (block.State.description);
        }
    }

	private string ApplyTags(string s)
	{
		string resourceName = "";
		do {
			resourceName = GetNextResourceName (ref s);
		} while(resourceName != "");


        int i = 0;
        foreach(Condition c in showingBlock.State.conditions)
        {
            ReplaceNextConditionTag(ref s, i, c.ChechCondition(showingBlock)>0);
            i++;
        }
		return s;
	}

    private void ReplaceNextConditionTag(ref string s, int i, bool condition)
    {
        if (s.Contains("<if" + i + ">"))
        {
            if (condition)
            {
                s = s.Replace("</if" + i + ">", "</color></b>");
                s = s.Replace("<if" + i + ">", "<b><color=#FFFFFF>");
            }
            else {
                s = s.Replace("</if" + i + ">", "</color>");
                s = s.Replace("<if" + i + ">", "<color=#a9a9ad>");
            }
        }
    }

    private string GetNextResourceName(ref string s)
	{
		
		string result = "";

		if(s.Contains("["))
		{
			int first = s.IndexOf ("[");
			int second = s.IndexOf ("]");

			result = s.Substring (first, second-first+1);


            string resName = result.Substring(1, result.Length-2);

            List<GameResource> resourcesList = new List<GameResource>();
            foreach (Inkome ink in ResourcesManager.Instance.StartedReources)
            {
                resourcesList.Add(ink.resource);
            }

			s = s.Replace (result, "<sprite index="+ resourcesList.IndexOf(resourcesList.Find(r=>r.name == resName)) +">");
			result = result.Substring (1, result.Length-2);
		}

		return result;
	}
}
