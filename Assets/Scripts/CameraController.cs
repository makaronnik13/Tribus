using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	Vector3 aimPosition = Vector3.zero;

	// Use this for initialization
	void Start () {
		CardsPlayer.Instance.OnAimsChanged += Changed;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, aimPosition, Time.deltaTime);
		transform.Rotate (Vector3.up*-Input.GetAxis("Horizontal"));
		transform.GetChild(0).GetComponent<Camera>().fieldOfView -= Input.GetAxis ("Vertical");
        transform.GetChild(0).GetComponent<Camera>().fieldOfView = Mathf.Clamp (transform.GetChild(0).GetComponent<Camera>().fieldOfView, 15, 75);
	}

	void Changed (List<ISkillAim> aims)
	{
		Vector3 pos = Vector3.zero;
		int numberOfBlocks = 0;

		foreach(ISkillAim aim in aims)
		{
			if(aim.GetType()==typeof(Block))
			{
				pos += ((Block)aim).transform.position;
				numberOfBlocks++;
			}
		}

		if(numberOfBlocks == 0)
		{
			return;
		}
		pos /= numberOfBlocks;
		aimPosition = pos;
	}
}
