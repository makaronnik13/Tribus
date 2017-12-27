using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StatsPanel : MonoBehaviour {

	public Transform content;
	public GameObject rawPrefab;

	private void Awake()
	{
		StatsManager.Instance.OnStatChanged += Repaint;
	}

	private void Repaint()
	{
		Debug.Log ("repaint");
		foreach(Transform t in content)
		{
			Destroy(t.gameObject);
		}

		List<Inkome> incomes = StatsManager.Instance.incomes.OrderBy (i=>i.resource.Priority).ToList();

		foreach(Inkome inc in incomes)
		{
			if (inc.resource.showInPanel) {
				GameObject newRaw = Instantiate (rawPrefab, content);
				newRaw.transform.GetChild (0).GetComponent<Image> ().sprite = inc.resource.sprite;
				try {
					if (inc.resource.incoming) {
						newRaw.GetComponentInChildren<Text> ().text = inc.value + " (" + incomes.Find (i => i.resource == inc.resource).value + ")";
					} else {
						newRaw.GetComponentInChildren<Text> ().text = incomes.Find (i => i.resource == inc.resource).value + "";
					}
				} catch {

				}
			}
		}
	}
}
