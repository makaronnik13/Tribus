using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tribus
{
public class MapCamera : MonoBehaviour {

	// Update is called once per frame
	void Update ()
    {
        RoomMap rm = FindObjectOfType<RoomMap>();
        transform.position = Vector3.Lerp(transform.position, new Vector3(rm.CurrentPartyPosition.x, rm.CurrentPartyPosition.y, -10),Time.deltaTime);
	}
}
}
