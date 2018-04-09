using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ColorChooser : MonoBehaviour {

	public Image ColorImage;

	private Color[] Colors
	{
		get
		{
			return DefaultResourcesManager.Colors;
		}
	}

	private int id;
	private int Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
			if(value>=Colors.Length)
			{
				id = 0;
			}
			if(value<0)
			{
				id = Colors.Length-1;
			}
		}
	}

	public void Left()
	{
		Id--;
		UpdateColor();
	}

	public void Right()
	{
		Id++;
		UpdateColor();
	}

	void OnEnable()
	{
		Id = Colors.ToList().IndexOf(LobbyPlayerIdentity.Instance.player.PlayerColor);
		UpdateColor();
	}

	private void UpdateColor()
	{
		ColorImage.color = Colors [id];
		LobbyPlayerIdentity.Instance.player.PlayerColor = Colors[Id];
	}
}
