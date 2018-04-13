using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerVisual : Photon.MonoBehaviour, ISkillAim 
{
	public Image selector;
	public Image avatar;
	public Image border;
	public GameObject playerInfo;
	public GameObject portrait;

	public Player Player
	{
		get
		{
			return null;
		}
	}

	public void Init(float[] playerColor, int v)
	{
        GetComponent<PhotonView>().RPC("InitOnClient", PhotonTargets.AllBuffered, new object[] {  playerColor,v});
	}

    [PunRPC]
    private void InitOnClient(float[] c, int v)
    {
        Color playerColor = new Color(c[0], c[1], c[2]);
        avatar.sprite = DefaultResourcesManager.Avatars[v];
        selector.enabled = false;
        border.color = playerColor;
        selector.material = new Material(selector.material);
        transform.SetParent(LocalPlayerLogic.Instance.visual.playersVisualiser.transform);
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }

	public void SetActivePlayer(bool v)
	{
		if (v) 
		{
			transform.SetAsLastSibling ();
			portrait.transform.localScale = Vector3.one * 1.5f;
		} else 
		{
			transform.SetAsFirstSibling ();
			portrait.transform.localScale = Vector3.one;
		}
	}

	public bool IsAwaliable(Card card)
	{
		if (card) 
		{
			CardEffect cardEffect = card.CardEffects.FirstOrDefault (ce=>ce.cardAim == CardEffect.CardAim.Player);
			if(cardEffect!=null)
			{
				if(cardEffect.playerAimType == CardEffect.PlayerAimType.All)
				{
					return true;
				}

				if(cardEffect.playerAimType == CardEffect.PlayerAimType.Any)
				{
					return true;
				}

				if(cardEffect.playerAimType == CardEffect.PlayerAimType.Enemies && Player!=GameLobby.Instance.CurrentPlayer)
				{
					return true;
				}

				if(cardEffect.playerAimType == CardEffect.PlayerAimType.Enemy && Player!=GameLobby.Instance.CurrentPlayer)
				{
					return true;
				}

				if(cardEffect.playerAimType == CardEffect.PlayerAimType.You && Player==GameLobby.Instance.CurrentPlayer)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void Highlight(Card card, bool v)
	{
		if (v) 
		{
			selector.enabled = true;
			selector.material.color = Color.yellow;
		} else 
		{
			selector.enabled = false;
		}
	}

	public void HighlightSelected(Card card, bool v)
	{
		if (v && IsAwaliable (card)) 
		{
			selector.material.color = Color.red;	
		}

		if(!v)
		{
			if (IsAwaliable(card)) 
			{
				selector.material.color = Color.yellow;
			} 
			else 
			{
				selector.enabled = false;
			}
		}
	}

	public void HighlightSimple(bool v)
	{
		//playerInfo.SetActive (v);
	}

	void OnMouseEnter()
	{
		if (CardsPlayer.Instance.ActiveCard == null) {
			HighlightSimple (true);
		} else 
		{
			CardEffect cardEffect = CardsPlayer.Instance.ActiveCard.CardEffects.FirstOrDefault (ce=>ce.cardAim == CardEffect.CardAim.Player);

			if(cardEffect!=null)
			{
				if(cardEffect.playerAimType == CardEffect.PlayerAimType.Any)
				{

					CardsPlayer.Instance.SelectAim(this);
				}

				if(cardEffect.playerAimType == CardEffect.PlayerAimType.Enemy && Player!=GameLobby.Instance.CurrentPlayer)
				{

					CardsPlayer.Instance.SelectAim (this);
				}
			}

			cardEffect = CardsPlayer.Instance.ActiveCard.CardEffects.FirstOrDefault (ce=>ce.cardAim == CardEffect.CardAim.Cell);

			if(cardEffect!=null)
			{
				if(cardEffect.cellAimType != CardEffect.CellAimType.All  && cardEffect.cellAimType != CardEffect.CellAimType.Random)
				{

					CardsPlayer.Instance.SelectAim(this);
				}
			}
		}
	}

	void OnMouseExit()
	{
		HighlightSimple (false);

		bool shouldDehighlight = true;

		if (CardsPlayer.Instance.ActiveCard) {
			CardEffect cardEffect = CardsPlayer.Instance.ActiveCard.CardEffects.FirstOrDefault (ce => ce.cardAim == CardEffect.CardAim.Player);
			if (cardEffect != null) {
				if (cardEffect.playerAimType == CardEffect.PlayerAimType.All || cardEffect.playerAimType == CardEffect.PlayerAimType.Enemies || cardEffect.playerAimType == CardEffect.PlayerAimType.You) {
					shouldDehighlight = false;
				}
			}


			cardEffect = CardsPlayer.Instance.ActiveCard.CardEffects.FirstOrDefault (ce=>ce.cardAim == CardEffect.CardAim.Cell);

			if(cardEffect!=null)
			{
				if(cardEffect.cellAimType == CardEffect.CellAimType.All || cardEffect.cellAimType == CardEffect.CellAimType.Random)
				{
					shouldDehighlight = false;
				}
			}
		}


		if(shouldDehighlight)
		{
			CardsPlayer.Instance.SelectAim (null);
		}
	}
}
