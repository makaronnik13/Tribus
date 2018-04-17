using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTextMeshProPositioning : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;
    }
	
}
