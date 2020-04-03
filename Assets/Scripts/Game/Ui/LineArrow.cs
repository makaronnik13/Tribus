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

			transform.position = CardsPlayer.Instance.ActiveCard.transform.position;

			lr.enabled = true;
			Vector3 endPosition = GetAimPosition ();
			endPosition = transform.InverseTransformPoint(endPosition);


			Vector3[] points = new Vector3[]{Vector3.zero, endPosition/2+Vector3.up*curving* Mathf.Abs(endPosition.x), endPosition };


		

			points = LineArrow.MakeSmoothCurve (points, 10);

			lr.positionCount = points.Length;
			lr.SetPositions (points);

			tip.transform.localPosition = endPosition;

			Vector3 diff = points[points.Length-5] - endPosition;
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
			if(ce.cardAim != CardEffect.CardAim.None)
			{
				if(ce.cardAim != CardEffect.CardAim.All && ce.cardAim != CardEffect.CardAim.Allies && ce.cardAim != CardEffect.CardAim.Enemies && ce.cardAim != CardEffect.CardAim.You)
				{
					v = true;	
				}
			}
		}

		return v;
	}

	private Vector3 GetAimPosition()
	{
		var ray = guiCamera.ScreenPointToRay(Input.mousePosition);
		var plane = new Plane(transform.forward, (transform.position-guiCamera.transform.position).z* transform.forward);

			float rayDistance;
			if (plane.Raycast(ray, out rayDistance))
			{
				return ray.GetPoint(rayDistance);

			}
		return Vector3.zero;
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

	public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve,float smoothness){
		List<Vector3> points;
		List<Vector3> curvedPoints;
		int pointsLength = 0;
		int curvedLength = 0;

		if(smoothness < 1.0f) smoothness = 1.0f;

		pointsLength = arrayToCurve.Length;

		curvedLength = (pointsLength*Mathf.RoundToInt(smoothness))-1;
		curvedPoints = new List<Vector3>(curvedLength);

		float t = 0.0f;
		for(int pointInTimeOnCurve = 0;pointInTimeOnCurve < curvedLength+1;pointInTimeOnCurve++){
			t = Mathf.InverseLerp(0,curvedLength,pointInTimeOnCurve);

			points = new List<Vector3>(arrayToCurve);

			for(int j = pointsLength-1; j > 0; j--){
				for (int i = 0; i < j; i++){
					points[i] = (1-t)*points[i] + t*points[i+1];
				}
			}

			curvedPoints.Add(points[0]);
		}

		return(curvedPoints.ToArray());
	}
}
