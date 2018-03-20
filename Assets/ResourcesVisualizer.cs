using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesVisualizer : MonoBehaviour {

    public GameObject ResourceValueVisualizer;
    private Dictionary<GameResource, GameObject> visualisers = new Dictionary<GameResource, GameObject>();

	// Use this for initialization
	void Awake ()
    {
        List<GameResource> res = new List<GameResource>();
        foreach (Inkome ink in ResourcesManager.Instance.StartedReources)
        {
            res.Add(ink.resource);
        }

        foreach (GameResource gr in res.OrderBy(gr => gr.Priority))
        {
            if (!gr.showInPanel)
            {
                continue;
            }
            GameObject newVisualiser = Instantiate(ResourceValueVisualizer, Vector3.zero, Quaternion.identity, transform);
            newVisualiser.transform.localScale = Vector3.one;
            newVisualiser.GetComponent<Image>().sprite = gr.sprite;
            visualisers.Add(gr, newVisualiser);
            newVisualiser.SetActive(false);
        }
        ResourcesManager.Instance.OnResourceValueChanged += ValueChanged;
	}

    private void ValueChanged(GameResource gr, int v)
    {
        visualisers[gr].SetActive(v!=0);
        visualisers[gr].GetComponentInChildren<TextMeshProUGUI>().text = "" + v;
    }
}
