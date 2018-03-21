using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineArrow : MonoBehaviour {

	private LineRenderer lr;

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent.childCount > 1) {
			lr.enabled = true;
			lr.SetPosition (0, transform.position);
			lr.SetPosition (1, GetAimPosition ());
		} else 
		{
			lr.enabled = false;
		}
	}

	private Vector3 GetAimPosition()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit)) 
		{
			return hit.point;
		}

		return ray.origin + ray.direction * Vector3.Distance (Camera.main.transform.position, GetComponentInParent<Canvas>().transform.position);

	}
}
