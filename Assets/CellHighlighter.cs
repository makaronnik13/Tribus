using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellHighlighter : MonoBehaviour {

    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    private void Awake()
    {
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            renderers.Add(sr);
            sr.enabled = false;
        }
    }

    public void Set(bool v, bool type)
    {
        if (type)
        {
            renderers[1].enabled = v;
        }
        else
        {
            renderers[0].enabled = v;
        }
    }
}
