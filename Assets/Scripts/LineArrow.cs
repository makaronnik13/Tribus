using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineArrow : MonoBehaviour {

	private LineRenderer lr;
	public GameObject tooth;

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent.childCount > 1) {
			lr.enabled = true;
			tooth.SetActive (true);
			lr.SetPosition (0, transform.position);
			lr.SetPosition (1, GetAimPosition ());
			Vector3 camPos = Camera.main.transform.position;
			lr.endWidth = lr.startWidth*(Vector3.Distance(camPos, GetAimPosition ())/Vector3.Distance(camPos, transform.position))/2;
			tooth.transform.position = GetAimPosition ();
			tooth.transform.localScale = 100000 * lr.endWidth*Vector3.one;
			tooth.transform.rotation = Quaternion.LookRotation (transform.position-GetAimPosition ());
		} else 
		{
			lr.enabled = false;
			tooth.SetActive (false);
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
