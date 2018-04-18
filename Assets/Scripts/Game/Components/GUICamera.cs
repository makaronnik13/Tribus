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
}
