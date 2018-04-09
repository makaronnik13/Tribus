using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameChooser : MonoBehaviour {

	public TMP_InputField NameField;

	private string[] Names
	{
		get
		{
			return DefaultResourcesManager.Names;
		}
	}

	void OnEnable()
	{
		NameField.text = LobbyPlayerIdentity.Instance.player.PlayerName;
	}

	public void UpdateName()
	{
		LobbyPlayerIdentity.Instance.player.PlayerName = NameField.text;
	}

	public void RandomizeName()
	{
		NameField.text = Names [Random.Range(0, Names.Length - 1)];
		UpdateName ();
	}
}
