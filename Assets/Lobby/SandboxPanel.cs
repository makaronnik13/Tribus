using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SandboxPanel : MonoBehaviour {

	public int MinSize = 2;
	public int MaxSize = 8;

	public TextMeshProUGUI Counter;
	public TMP_Dropdown DeckDropdown;
	public Button minusButton, plusButton;

	private int _count = 3;
	private int count
	{
		get
		{
			return _count;
		}
		set
		{
			_count = value;
			_count = Mathf.Clamp (_count, MinSize, MaxSize);
			minusButton.interactable = (_count!=MinSize);
			plusButton.interactable = (_count!=MaxSize);
			Counter.text = _count.ToString ();
		}
	}

	void OnEnable()
	{
		count = count;
		DeckDropdown.ClearOptions ();
		DeckDropdown.AddOptions (LobbyPlayerIdentity.Instance.player.Decks.Where(d=>d.Awaliable).Select(d=>d.DeckName).ToList());
	}

	public void CreateGame()
	{
		Debug.Log ("create game "+count);
	}

	public void Back()
	{
		LobbyMenu.Instance.BackToMainMenu ();
	}

	public void Minus()
	{
		count--;
	}

	public void Plus()
	{
		count++;
	}
}
