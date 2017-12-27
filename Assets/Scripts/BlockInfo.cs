using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BlockInfo : MonoBehaviour {

	private GameObject canvas;
	public GameObject infoBlockRow;
	public Transform content;
    public GameObject popupInfo;

	private bool showing = false;

	// Use this for initialization
	void Start () {
		canvas = transform.GetChild (0).GetChild(0).gameObject;
		canvas.SetActive (false);
	}
	
	// Update is called once per frame
	public void Show (Block b) 
	{
		if(showing || b.CurrentIncome.Count==0)
		{
			return;
		}

		foreach(Transform t in content)
		{
			Destroy (t.gameObject);
		}

			
		foreach(Inkome inc in b.CurrentIncome.OrderBy (i=>i.resource.Priority).ToList())
		{
			GameObject newRaw = Instantiate (infoBlockRow, content);
			newRaw.GetComponentInChildren<Image> ().sprite = inc.resource.sprite;

			newRaw.GetComponentInChildren<Text> ().text = inc.value+"";
		}
		canvas.SetActive (true);
		showing = true;
	}

    public void Emmit(Block b)
    {
        if (b.CurrentIncome.Count == 0)
        {
            return;
        }
  

		foreach (Inkome inc in b.CurrentIncome.OrderBy (i=>i.resource.Priority).ToList())
        {
            if (inc.resource.incoming)
            {
                GameObject newRaw = Instantiate(infoBlockRow, popupInfo.transform);
                newRaw.GetComponentInChildren<Image>().sprite = inc.resource.sprite;

                newRaw.GetComponentInChildren<Text>().text = inc.value + "";
            }
        }

        popupInfo.GetComponent<PopupInfoPanel>().Emmit();
    }

    public void Hide()
	{
		if(!showing)
		{
			return;
		}
		canvas.SetActive (false);
		showing = false;
	}
}
