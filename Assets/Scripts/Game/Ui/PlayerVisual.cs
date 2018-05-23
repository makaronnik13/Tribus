using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;

public class PlayerVisual : Photon.MonoBehaviour 
{
    public string PlayerName;

	public List<string> Hand = new List<string>();
	public Stack<string> Drop = new Stack<string>();
	public Queue<string> Pile = new Queue<string>();


	public TextMeshProUGUI PlayerNameField;
	public Image PlayerWarriorPortrait;
	public HpSlider PlayerHpSlider;

	private Warrior warrior;
	public Warrior Warrior
	{
		get
		{
			return warrior;
		}
	}

	private Color color;
    public Color Color
    {
        get
        {
            return color;
        }
    }

	public PhotonPlayer Player
	{
		get
		{
            return GetComponent<PhotonView>().owner;
		}
	}

	public void Init(string name, float[] playerColor, int v, string[] cards, bool activePlayer)
	{
		GetComponent<PhotonView>().RPC("InitOnClient", PhotonTargets.AllBuffered, new object[] {name,  playerColor,v, cards, activePlayer});
	}

    [PunRPC]
	private void InitOnClient(string name, float[] c, int v, string[] cards, bool activePlayer)
    {
		warrior = DefaultResourcesManager.Warriors[v];
		foreach (string cardId in cards)
        {
            Pile.Enqueue(cardId);
        }
        PlayerName = name;
		color = new Color(c[0], c[1], c[2]);
        transform.SetParent(LocalPlayerLogic.Instance.visual.playersVisualiser.transform);
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
		PlayerNameField.text = PlayerName;
		PlayerWarriorPortrait.sprite = warrior.sprite;
		PlayerHpSlider.Init (warrior.hp);
    }
		
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			//string pName = PlayerName;
			//stream.Serialize(ref pName);
		}
		else
		{
			//Vector3 pos = Vector3.zero;
			//stream.Serialize(ref pos);  // pos gets filled-in. must be used somewhere
		}
	}
	

	public void RemoveCardsFromPile(List<string> cardsIds)
    {
		foreach(string id in cardsIds)
		{
			List<string> newPile = Pile.ToList ();
			newPile.Remove (id);
			Pile = new Queue<String> (newPile);
		}
    }

    public void AddCardsToHand(string[] cardsIds)
    {
        foreach (string cardId in cardsIds)
        {
            AddCardToHand(cardId);
        }
    }

    public void AddCardToHand(string cardId)
    {
        Hand.Add(cardId);
    }

	public void RemoveCardsFromDrop(List<string>  cardsIds)
    {
		foreach(string id in cardsIds)
		{
			List<string> newDrop = Drop.ToList ();
			newDrop.Remove (id);
			Drop = new Stack<String> (newDrop);
		}
    }

    public void AddCardsToPile(string[] cardsIds)
    {
        foreach (string cardId in cardsIds)
        {
            AddCardToPile(cardId);
        }
    }

    public void AddCardToPile(string cardId)
    {
        Pile.Enqueue(cardId);
    }

    public void AddCardsToDrop(string[] cardsIds)
    {
        foreach (string cardId in cardsIds)
        {
            AddCardToDrop(cardId);
        }
    }

    public void AddCardToDrop(string cardId)
    {
        Drop.Push(cardId);
    }

	public void RemoveCardsFromHand(string[] cardsIds)
    {
		foreach(string id in cardsIds)
		{
			Hand.Remove(id);
		}
    }
}
