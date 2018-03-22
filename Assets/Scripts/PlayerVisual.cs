using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisual : MonoBehaviour, ISkillAim 
{
	public MeshRenderer selector;
	public Image avatar;
	public Image border;

	private Player player;
	public Player Player
	{
		get
		{
			return player;
		}
	}

	public void Init(Player player)
	{
		this.player = player;
		avatar.sprite = player.avatar;
		selector.enabled = false;
		border.color = player.color;
	}

	public void SetActivePlayer(bool v)
	{
		if (v) 
		{
			transform.SetAsLastSibling ();
			transform.localScale = Vector3.one * 1.5f;
		} else 
		{
			transform.SetAsFirstSibling ();
			transform.localScale = Vector3.one;
		}
	}

	public bool IsAwaliable(Card card)
	{
		if (card) {
			return card.aimType == Card.CardAimType.Player;
		}
		return false;
	}

	public void Highlight(Card card, bool v)
	{
		if (v) 
		{
			selector.enabled = true;
			selector.GetComponent<MeshRenderer>().material.color = Color.yellow;
		} else 
		{
			selector.enabled = false;
		}
	}

	public void HighlightSelected(Card card, bool v)
	{
		if (v && IsAwaliable (card)) 
		{
			selector.GetComponent<MeshRenderer>().material.color = Color.red;	
		}

		if(!v)
		{
			if (IsAwaliable(card)) 
			{
				selector.GetComponent<MeshRenderer>().material.color = Color.yellow;
			} 
			else 
			{
				selector.enabled = false;
			}
		}
	}

	void OnMouseEnter()
	{
		Debug.Log ("Mouse enter");
	}

	void OnMouseExit()
	{
		Debug.Log ("Mouse exit");
	}
}
