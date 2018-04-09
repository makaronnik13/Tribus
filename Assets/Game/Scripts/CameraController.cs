using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController> {

	Vector3 aimPosition = Vector3.zero;

	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, aimPosition, Time.deltaTime);
		transform.Rotate (Vector3.up*-Input.GetAxis("Horizontal"));
		transform.GetChild(0).GetComponent<Camera>().fieldOfView -= Input.GetAxis ("Vertical");
        transform.GetChild(0).GetComponent<Camera>().fieldOfView = Mathf.Clamp (transform.GetChild(0).GetComponent<Camera>().fieldOfView, 15, 75);
		//transform.GetChild(1).GetComponent<Camera>().fieldOfView -= Input.GetAxis ("Vertical");
		//transform.GetChild(1).GetComponent<Camera>().fieldOfView = Mathf.Clamp (transform.GetChild(0).GetComponent<Camera>().fieldOfView, 15, 75);
	}

	public void AimedBlockChanged (Block aim)
	{
		aimPosition = aim.transform.position;
	}
}
