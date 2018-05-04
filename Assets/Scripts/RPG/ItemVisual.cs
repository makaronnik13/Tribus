using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Tribus;

public class ItemVisual : MonoBehaviour {

	public List<PhotonPlayer> players = new List<PhotonPlayer>();

    public object item;

	public Image ItemImage;
	public TextMeshProUGUI Counter;

	public void MouseEnter()
	{
        GetComponentInParent<ChestPanel>().ItemHovered(this);
		Highlught (true);
	}

	public void MouseExit()
	{
        GetComponentInParent<ChestPanel>().ItemUnHovered(this);
        Highlught (false);
	}

	public void MouseClick()
	{
		if (players.Contains (PhotonNetwork.player)) 
		{
            //GetComponentInParent<ChestPanel>().ItemClicked(this, false);
            players.Remove (PhotonNetwork.player);
			Choose (false);
		} else 
		{
			players.Add (PhotonNetwork.player);
			Choose (true);
            //GetComponentInParent<ChestPanel>().ItemClicked(this, true);
        }
	}

	public void Init(Card card)
	{
        item = card;
		Counter.enabled = false;
		ItemImage.sprite = Resources.Load<Sprite>("Sprites/RPG/Card");
        ItemImage.material = new Material(ItemImage.material);
    }

	public void Init(int gold)
	{
        item = gold;
		Counter.text = gold.ToString ();
		ItemImage.sprite = Resources.Load<Sprite>("Sprites/RPG/Gold");
		Counter.enabled = true;
        ItemImage.material = new Material(ItemImage.material);
    }

	public void Init(Item item)
	{
        this.item = item;
		Counter.enabled = false;
		ItemImage.sprite = item.ItemSprite;
        ItemImage.material = new Material(ItemImage.material);
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

    public void GiveToPlayer(PhotonPlayer winer)
    {
        Vector3 aimPosition = MapCanvas.Instance.InventoryButton.transform.position;
        if (winer!=PhotonNetwork.player)
        {
            aimPosition = MapCanvas.Instance.OutTransform.position;
        }

        StartCoroutine(MoveItemTo(aimPosition, 1));
    }

    private IEnumerator MoveItemTo(Vector3 aimPosition, int v)
    {
        transform.SetParent(GetComponentInParent<Canvas>().transform);
        float time = 0;
        while (time <= v)
        {
            transform.position = Vector3.Lerp(transform.position, aimPosition, time/v);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, time / v);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(gameObject);
        yield return null;
    }
}