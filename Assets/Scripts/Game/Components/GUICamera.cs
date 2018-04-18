using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUICamera : Singleton<GUICamera>{

	private Camera guiCamera;
	public Camera GuiCamera
	{
		get
		{
			if(!guiCamera)
			{
				guiCamera = GetComponent<Camera> ();
			}
			return guiCamera;
		}
	}

	void Awake()
	{
		transform.localRotation = transform.parent.GetChild (0).transform.localRotation;
	}
}
