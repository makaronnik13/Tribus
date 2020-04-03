using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeHpPartice : MonoBehaviour {

	public float speed = 0.5f;
	private TextMeshProUGUI text;

	// Use this for initialization
	public void Init (int v, bool toBlock = false) 
	{
		text = GetComponentInChildren<TextMeshProUGUI> ();
		text.text = "";

		if (v > 0) 
		{
			text.color = Color.green;
			text.text = "+";
		} else 
		{
			text.color = Color.red;
		}

		if(toBlock)
		{
			text.color = Color.blue;	
		}

		text.text += v;
		Destroy (gameObject, 1f/speed);
		transform.Translate (Vector3.right*Random.Range(0f,1f));
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (text) 
		{
			transform.Translate (Vector3.up * Time.deltaTime);
			text.color = Color.Lerp (text.color, new Color(text.color.r, text.color.g, text.color.b, 0), Time.deltaTime*speed);
			transform.localScale = Vector3.Lerp (Vector3.one, Vector3.one*1.5f, Time.deltaTime*speed);
		}
	}
}
