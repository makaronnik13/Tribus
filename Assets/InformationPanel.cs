using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanel : Singleton<InformationPanel>
{
    public Image icon;
    public Text blockName;
    public Transform incomeContent;
    public TextMesh symbiosys;
    public GameObject incomePrefab;

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
        }
        else
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
        }
    }
}
