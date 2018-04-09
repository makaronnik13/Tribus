using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class PlayerInfo : MonoBehaviour {

	public TextMeshProUGUI PlayerName, Drop, Hand, Pile, Hexes;
	public TextMeshProUGUI[] Resources;

	void OnEnable()
	{
		Player visualPlayer = GetComponentInParent<PlayerVisual> ().Player;

		int[] values = new int[]{0,0,0};

		foreach(Inkome ink in ResourcesManager.Instance.GetIncomeForPlayer (visualPlayer).OrderBy(inkome=>inkome.resource.Priority))
		{
			values[ink.resource.Priority] += ink.value;
		}

		Resources [0].text = "" + values [0];
		Resources [1].text = "" + values [1];
		Resources [2].text = "" + values [2];

		PlayerName.text = visualPlayer.PlayerName;
		PlayerName.color = visualPlayer.PlayerColor;
		Drop.text = ""+visualPlayer.Drop.Count ();
		Hand.text = "" + visualPlayer.Hand.Count;
		Pile.text = "" + (visualPlayer.Pile.Count ());
		Hexes.text = BlocksField.Instance.Blocks.Where (b=>b.Owner == visualPlayer).Count()+"";
	}
}
