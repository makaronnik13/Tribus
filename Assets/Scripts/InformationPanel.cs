﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InformationPanel : Singleton<InformationPanel>
{
    public Image icon;
    public Text blockName;
    public Transform incomeContent;
    public TMPro.TextMeshProUGUI symbiosys;
    public GameObject incomePrefab;
	private Block showingBlock;

	// Use this for initialization
	void Start () {
        GetComponent<Canvas>().enabled = false;
        symbiosys.gameObject.SetActive(false);
    }
	
    public void ShowInfo(Block block)
    {
        if (block == null)
        {
            GetComponent<Canvas>().enabled = false;
            symbiosys.gameObject.SetActive(false);
            showingBlock = block;
        }
		else if(showingBlock!=block)
        {
            GetComponent<Canvas>().enabled = true;
            symbiosys.gameObject.SetActive(true);
            foreach (Transform t in incomeContent)
            {
                Destroy(t.gameObject);
            }
            blockName.text = block.State.StateName;
            icon.sprite = block.State.Sprite;

            foreach (Inkome inc in block.State.income)
            {
                GameObject newRaw = Instantiate(incomePrefab, incomeContent);
                newRaw.transform.localScale = Vector3.one;
                newRaw.GetComponentInChildren<Image>().sprite = inc.resource.sprite;
                newRaw.GetComponentInChildren<Text>().text = inc.value + "";
            }

            showingBlock = block;
            symbiosys.text = ApplyTags (block.State.description);
        }
    }

	private string ApplyTags(string s)
	{
		List<Material> materials = new List<Material> ();

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


			s = s.Replace (result, "<sprite index="+StatsManager.Instance.resources.ToList().IndexOf(StatsManager.Instance.resources.ToList().Find(r=>r.name == resName)) +">");
			result = result.Substring (1, result.Length-2);
		}

		return result;
	}
}
