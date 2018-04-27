using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineArrow : MonoBehaviour {

	public float curving;

	public Transform tip;

	private LineRenderer lr;
	private Camera guiCamera
	{
		get
		{
			return GUICamera.Instance.GuiCamera;
		}
	}

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (CardsPlayer.Instance.ActiveCard && ShowAimForCard(CardsPlayer.Instance.ActiveCard.CardAsset)) 
		{
			lr.enabled = true;
			Vector3 endPosition = GUICamera.Instance.GuiCamera.WorldToScreenPoint(GetAimPosition ())- GUICamera.Instance.GuiCamera.WorldToScreenPoint(transform.parent.position);
			endPosition.z = -50;

			endPosition -= endPosition.normalized*125;
			List<Vector3> points = new List<Vector3> ();


			points.Add (Vector3.zero);
			points.Add (endPosition);
			lr.positionCount = 2;
			lr.SetPositions (points.ToArray());

			tip.transform.localPosition = endPosition;

			Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - endPosition;
			diff.Normalize();
			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;


			tip.transform.localRotation = Quaternion.Euler(0f, 0f, rot_z + 90);
			tip.gameObject.SetActive (true);
		} else 
		{
			lr.enabled = false;
			tip.gameObject.SetActive (false);
		}
	}

	private bool ShowAimForCard(Card c)
	{
		bool v = false;
		if(c == null)
		{
			return v;
		}
		foreach(CardEffect ce in c.CardEffects)
		{
			

			if(ce.cardAim == CardEffect.CardAim.Player)
			{
				if(ce.playerAimType != CardEffect.PlayerAimType.All && ce.playerAimType != CardEffect.PlayerAimType.Enemies && ce.playerAimType != CardEffect.PlayerAimType.You)
				{
					v = true;	
				}
			}
		}

		return v;
	}

	private Vector3 GetAimPosition()
	{
		if(CardsPlayer.Instance.focusedAims.Count == 1)
		{
			return (CardsPlayer.Instance.focusedAims [0] as MonoBehaviour).transform.position;
		}

		RaycastHit hit;
		Ray ray = guiCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit)) 
		{
			return hit.point;
		}

		return ray.origin + ray.direction * Vector3.Distance (guiCamera.transform.position, GetComponentInParent<Canvas>().transform.position);

	}

	public Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			oneMinusT * oneMinusT * oneMinusT * p0 +
			3f * oneMinusT * oneMinusT * t * p1 +
			3f * oneMinusT * t * t * p2 +
			t * t * t * p3;
	}
}
