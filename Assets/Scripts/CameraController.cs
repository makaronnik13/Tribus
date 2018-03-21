using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	Vector3 aimPosition = Vector3.zero;

	// Use this for initialization
	void Start () {
		//SkillsController.Instance.onHighlightedBlockChanged += Changed;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, aimPosition, Time.deltaTime);
		transform.Rotate (Vector3.up*-Input.GetAxis("Horizontal"));
		transform.GetChild(0).GetComponent<Camera>().fieldOfView -= Input.GetAxis ("Vertical");
        transform.GetChild(0).GetComponent<Camera>().fieldOfView = Mathf.Clamp (transform.GetChild(0).GetComponent<Camera>().fieldOfView, 15, 75);
	}

	void Changed (Block block)
	{
		if(block)
		{
			aimPosition = block.transform.position;
		}
	}
}
