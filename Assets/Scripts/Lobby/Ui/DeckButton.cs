using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour {

	public TextMeshProUGUI DeckName;
	public Image[] WinCardsImages;

	private DeckStruct cardStruct;

	public void Init(DeckStruct ds)
	{
		cardStruct = ds;
		DeckName.text = ds.DeckName;

		Sprite[] winSprites = ds.GetWinCardsSprires ();
		for(int i = 0; i<3;i++)
		{
			if (winSprites [i]) 
			{
				WinCardsImages [i].enabled = true;
				WinCardsImages[i].sprite = winSprites[i];
			} 
			else 
			{
				WinCardsImages [i].enabled = false;
			}
		}

	}

	public void Click()
	{
		LobbyMenu.Instance.EditDeck(cardStruct);
	}
}
