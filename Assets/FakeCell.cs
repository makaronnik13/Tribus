using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeCell : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponentInChildren<Image> ().sprite = GetComponentInParent<Block> ().State.Sprite;
	}
}
