using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour {

	public Transform content;
	public GameObject rawPrefab;

	private void Awake()
	{
		StatsManager.Instance.OnStatChanged += Repaint;
	}

	private void Repaint()
	{
		foreach(Transform t in content)
		{
			Destroy(t.gameObject);
		}
		List<Inkome> incomes = StatsManager.Instance.incomes;

		foreach(Inkome inc in StatsManager.Instance.currentResources)
		{
			GameObject newRaw = Instantiate (rawPrefab, content);
			newRaw.transform.GetChild(0).GetComponent<Image> ().sprite = inc.resource.sprite;
            try
            {
                if (inc.resource.incoming)
                {
                    newRaw.GetComponentInChildren<Text>().text = inc.value + " (" + incomes.Find(i => i.resource == inc.resource).value + ")";
                }
                else
                {
                    newRaw.GetComponentInChildren<Text>().text = incomes.Find(i => i.resource == inc.resource).value+"";
                }
            }
            catch
            {

            }
		}
	}
}
