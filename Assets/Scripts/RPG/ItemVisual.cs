using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Tribus;

public class ItemVisual : MonoBehaviour {

    public object item;
	public Image ItemImage;
	public TextMeshProUGUI Counter;
	private Action<ItemVisual> OnHovered, OnUnhovered, OnClick;
	public int currentCost;

	public void MouseEnter()
	{
		if (OnHovered!=null) 
		{
			OnHovered (this);
		}
	}

	public void MouseExit()
	{
		if(OnUnhovered!=null)
		{
			OnUnhovered (this);
		}
	}

	public void MouseClick()
	{
		if(OnClick!=null)
		{
			OnClick (this);
		}
	}
		

	public void Init(object item, Action<ItemVisual> onHovered, Action<ItemVisual> onUnhovered, Action<ItemVisual> onClick, int cost = 0)
	{
		this.OnClick += onClick;
		this.OnHovered += onHovered;
		this.OnUnhovered += onUnhovered;
		this.item = item;

		if(item.GetType()==typeof(Card))
		{
			currentCost = cost;
			Counter.enabled = cost!=0;
			Counter.text = cost+"";
			if (cost>PlayerStats.Instance.PlayerMoney) 
			{
				Counter.color = Color.red * 0.6f;
			}
			ItemImage.sprite = Resources.Load<Sprite>("Sprites/RPG/Card");		
		}
		if(item.GetType()==typeof(int))
		{
			Counter.text = item.ToString ();
			ItemImage.sprite = Resources.Load<Sprite>("Sprites/RPG/Gold");
			Counter.enabled = true;	
		}
		if(item.GetType()==typeof(Item))
		{
			currentCost = cost;
			Counter.text = cost+"";
			Counter.enabled = cost!=0;
			if (cost>PlayerStats.Instance.PlayerMoney) 
			{
				Counter.color = Color.red * 0.6f;
			}
			ItemImage.sprite = ((Item)item).ItemSprite;
		}
       
        ItemImage.material = new Material(ItemImage.material);
    }
		
	public void SetColor(int i)
	{
		switch(i)
		{
		case 0:
			ItemImage.material.SetFloat ("_OutlineSize", 0);
			break;
		case 1:
			ItemImage.material.SetColor ("_OutlineColor", new Color(1,1,0,0.75f));
			ItemImage.material.SetFloat ("_OutlineSize", 1);
			break;
		case 2:
			ItemImage.material.SetColor ("_OutlineColor", new Color(1,0,0,0.75f));
			ItemImage.material.SetFloat ("_OutlineSize", 1);
			break;
		}
	}

    public void GiveToPlayer(PhotonPlayer winer)
    {
        Vector3 aimPosition = MapCanvas.Instance.InventoryButton.transform.position;
        if (winer!=PhotonNetwork.player)
        {
            aimPosition = MapCanvas.Instance.OutTransform.position;
        }

		if (item.GetType () == typeof(int)) 
		{
			PlayerStats.Instance.PlayerMoney += (int)item;
		}

		if (item.GetType () == typeof(Card)) 
		{
			PlayerStats.Instance.PlayerCards.Add((Card)item);
		}

		if (item.GetType () == typeof(Item)) 
		{
			PlayerStats.Instance.PlayerItems.Add((Item)item);
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