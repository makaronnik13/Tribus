using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestVisual : MonoBehaviour {

    private List<Transform> itemsTransforms = new List<Transform>();

	public void ShowItems(List<GameObject> items)
    {
        itemsTransforms.Clear();
        foreach (GameObject go in items)
        {
            go.transform.SetParent(transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.one;
            itemsTransforms.Add(go.transform);
        }

        StartCoroutine(MoveItemsToPoints(0.2f));
    }

    private IEnumerator MoveItemsToPoints(float v)
    {
        float time = 0;
        while (time<=v)
        {
            for (int i = 0; i<itemsTransforms.Count; i++)
            {
                float yMultiplyer = -0.002f;
                int cards = itemsTransforms.Count;
                float fieldWidth = GetComponent<RectTransform>().rect.width*2;
                float cardWidth = 200;
                float offset = Mathf.Min(cardWidth, fieldWidth / cards);
                Vector3 aimPosition = Vector3.zero;
                float minOffset = -(cards - 1) * offset / 2;
                float yPos = -Mathf.Pow(minOffset + i* offset, 2) * yMultiplyer-200;
                aimPosition = new Vector3(minOffset + i * offset, yPos);
                itemsTransforms[i].localPosition = Vector3.Lerp(Vector3.zero, aimPosition, time/v);
                itemsTransforms[i].localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time/v);
            }
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return null;
    }
}
