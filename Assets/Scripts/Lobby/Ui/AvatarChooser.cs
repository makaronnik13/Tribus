using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class AvatarChooser : MonoBehaviour {

	public Image Image;
	public TextMeshProUGUI classText;

	private Sprite[] Avatars
	{
		get
		{
			return DefaultResourcesManager.Avatars;
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
			if(value>=Avatars.Length)
			{
				id = 0;
			}
			if(value<0)
			{
				id = Avatars.Length-1;
			}
		}
	}

	public void Left()
	{
		Id--;
		UpdateAvatar ();
	}

	public void Right()
	{
		Id++;
		UpdateAvatar ();
	}

	void OnEnable()
	{
		Id = LobbyPlayerIdentity.Instance.player.PlayerAvatarId;
		UpdateAvatar ();
	}

	private void UpdateAvatar()
	{
		Image.sprite = Avatars [id];
		classText.text = DefaultResourcesManager.Warriors[id].WarriorName;
		LobbyPlayerIdentity.Instance.player.PlayerAvatarId = Id;
	}
}
