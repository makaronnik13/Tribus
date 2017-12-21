using System;
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
    public TextMesh symbiosys;
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

			symbiosys.text = ApplyTags (block.State.description);
        }

		showingBlock = block;
    }

	private string ApplyTags(string s)
	{
		List<Material> materials = new List<Material> ();

		int i = 1;
		string resourceName = "";
		do {
			resourceName = GetNextResourceName (ref s, i);
			if(resourceName!="")
			{
				Material newMaterial = new Material(Shader.Find("Sprites/Default"));
				Debug.Log(resourceName);

				newMaterial.mainTexture = StatsManager.Instance.resources.ToList().Find(r=>r.name == resourceName).sprite.texture;
				materials.Add(newMaterial);
			}
			Debug.Log (resourceName);
			Debug.Log (s);
			i++;
		} while(resourceName != "");

		List<Material> newMAterials = new List<Material> ();
		newMAterials.Add (symbiosys.GetComponent<MeshRenderer> ().materials[0]);
		newMAterials.AddRange (materials);

		symbiosys.GetComponent<MeshRenderer> ().materials = newMAterials.ToArray();
		


		return s;
	}

	private string GetNextResourceName(ref string s, int i)
	{
		
		string result = "";

		if(s.Contains("["))
		{
			int first = s.IndexOf ("[");
			int second = s.IndexOf ("]");

			result = s.Substring (first, second-first+1);
			s = s.Replace (result, "<quad material="+i+"size=60 x=0.0 y=0.0 width=1 height=1 />");
			result = result.Substring (1, result.Length-2);
		}

		return result;
	}
}
