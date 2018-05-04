using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemVisual : MonoBehaviour {

	private List<PhotonPlayer> players = new List<PhotonPlayer>();

	public Image ItemImage;
	public TextMeshProUGUI Counter;

	public System.Action<ItemVisual> OnMouseEnter = (ItemVisual iv)=>{};
	public System.Action<ItemVisual> OnMouseExit = (ItemVisual iv)=>{};
	public System.Action<ItemVisual> OnMouseClick = (ItemVisual iv)=>{};

	public void MouseEnter()
	{
		Highlught (true);
	}

	public void MouseExit()
	{
		Highlught (false);
	}

	public void MouseClick()
	{
		if (players.Contains (PhotonNetwork.player)) 
		{
			players.Remove (PhotonNetwork.player);
			Choose (false);
		} else 
		{
			players.Add (PhotonNetwork.player);
			Choose (true);
		}
	}

	public void Init(Card card)
	{
		Counter.enabled = false;
		ItemImage.sprite = (Sprite)Resources.Load ("Sprites/RPG/Card");
	}

	public void Init(int gold)
	{
		Counter.text = gold.ToString ();
		ItemImage.sprite = (Sprite)Resources.Load ("Sprites/RPG/Gold");
		Counter.enabled = true;
	}

	public void Init(Item item)
	{
		Counter.enabled = false;
		ItemImage.sprite = item.ItemSprite;
	}


	private void Highlught(bool v)
	{
		if (v) 
		{
			ItemImage.material.SetFloat ("_OutlineSize", 1);
			ItemImage.material.SetColor ("_OutlineColor", new Color (1, 1, 0, 0.75f));	
		} 
		else 
		{
			if (players.Contains (PhotonNetwork.player)) 
			{
				ItemImage.material.SetColor ("_OutlineColor", new Color (1, 0, 0, 0.75f));
			} else 
			{
				ItemImage.material.SetFloat ("_OutlineSize", 0);
			}
		}

		//selector.material.SetColor ("_OutlineColor", new Color(1,0,0,0.75f));
	}

	private void Choose(bool v)
	{
		if (v) 
		{
			ItemImage.material.SetColor ("_OutlineColor", new Color(1,0,0,0.75f));
		} 
		else 
		{
			ItemImage.material.SetColor ("_OutlineColor", new Color(1,1,0,0.75f));
		}
		ItemImage.material.SetFloat ("_OutlineSize", 1);

	}
}