using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceImage : MonoBehaviour {

	public List<Sprite> diceSprites;
	public Image img;

	public void Init(ChellengeVariant.DiceSide side)
	{
		img.sprite = diceSprites[(int)side];
	}
}
