using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardsLayout : MonoBehaviour
{
    private RectTransform _rectTransform;
    private RectTransform rectTransform
    {
        get
        {
            if (!_rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
	private int childCount = 0;

	private List<Transform> childTransforms = new List<Transform>();

	public void Update()
	{
		if(childCount!=transform.childCount)
		{
			RecalculateSibling ();
			childCount = transform.childCount;
		}
	}

	public void RecalculateSibling()
	{
		childTransforms.Clear ();
		foreach(Transform t in transform)
		{
			childTransforms.Add (t);
		}

		StopCoroutine (LayoutCards());
		StartCoroutine (LayoutCards());
	}

	// Update is called once per frame
	IEnumerator LayoutCards () 
	{
		bool onPlace = false;
		float time = 0;

		while(!onPlace)
		{
			int i = 0;
			int cards = transform.childCount;
			float minRotation = -20;
			Vector3 minPosition = new Vector3 (GetComponent<RectTransform>().rect.width/2, 0,0);
			bool placed = true;

			foreach(Transform t in childTransforms)
			{
				float rotation = Mathf.Lerp(-minRotation, minRotation, i/(cards+0.0f));


				Quaternion aimRotation = Quaternion.Euler (new Vector3 (0, 0, rotation));
				Vector3 aimPosition = Vector3.Lerp (-minPosition, minPosition, i / (cards + 0.0f));

				if(t.localScale!=Vector3.one)
				{
					aimRotation = Quaternion.identity;
					aimPosition += Vector3.up * t.GetComponent<RectTransform>().rect.height/2;
				}

				Quaternion neededRotation = Quaternion.Lerp(t.rotation, aimRotation, Time.deltaTime);
				Vector3 neededPosition = Vector3.Lerp(t.localPosition, aimPosition, Time.deltaTime);

	
				if (neededPosition != t.localPosition || neededRotation != t.localRotation) {
					t.localRotation = neededRotation;
					t.localPosition = neededPosition;
					placed = false;
				} else 
				{
					t.GetComponent<CardVisual> ().FocusedEnabled = true;
				}
				i++;
			}

			onPlace = placed;
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
	}

	public int GetSibling(CardVisual cv)
	{
		for(int i = 0; i<childTransforms.Count;i++)
		{
			if(childTransforms[i].GetComponent<CardVisual>()==cv)
			{
				return i;
			}
		}
		return 0;
	}
}
