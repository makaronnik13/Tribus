using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockInfo : MonoBehaviour {

	private GameObject canvas;
	public GameObject infoBlockRow;
	public Transform content;

	private bool showing = false;

	// Use this for initialization
	void Start () {
		canvas = transform.GetChild (0).gameObject;
		canvas.SetActive (false);
	}
	
	// Update is called once per frame
	public void Show (Block b) 
	{
		foreach(Transform t in content)
		{
			Destroy (t.gameObject);
		}
		if(showing)
		{
			return;
		}
			
		foreach(Inkome inc in b.CurrentIncome)
		{
			GameObject newRaw = Instantiate (infoBlockRow, content);
			newRaw.GetComponentInChildren<Image> ().sprite = CombineModel.GetResourceImage(inc.resource);

			newRaw.GetComponentInChildren<Text> ().text = inc.value+"";
		}
		canvas.SetActive (true);
		showing = true;
		if(b.CurrentIncome.Count==0)
		{
			Hide ();
		}
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
