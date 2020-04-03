using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Tribus
{
    public class ShopPanel : MonoBehaviour
    {
        public GameObject ItemPrefab;
        public GameObject Visual;
        public TextMeshProUGUI ItemName;
        public TextMeshProUGUI ItemDescription;
        public CardVisual cardVisual;
        public Slider timeSlider;
		public Transform shopSlots;

        public float choosingTime = 30;

        public int testLevel;

        [Button("test")]
        public void Test()
        {
            FindObjectOfType<PlayerStats>().CurrentPartyLevel = testLevel;
            ShowPanel();
        }

        // Use this for initialization
        void Start()
        {
            FindObjectOfType<RoomActivator>().OnShopIn += ShowPanel;
        }

        private void ShowPanel()
        {
            timeSlider.maxValue = choosingTime;
            timeSlider.value = choosingTime;
            Visual.SetActive(true);
            CreateItems();
            StartCoroutine(Tick(choosingTime));
        }

        private void CreateItems()
        {
			List<object> avaliableItems = new List<object> ();

			foreach (Item it in FindObjectOfType<RoomMap> ().LevelSettingsAsset.Items) 
			{
				avaliableItems.Add (it);
			}

			Debug.Log (avaliableItems.Count());

			//temp fake. collect items from all players
			foreach (PlayerStats ps in FindObjectsOfType<PlayerStats>())
			{
				foreach (Item i in ps.PlayerItems)
				{
					avaliableItems.Remove(i);
				}
			}

			avaliableItems =  avaliableItems.OrderBy (c=>Guid.NewGuid()).ToList();
			avaliableItems = avaliableItems.Take (Mathf.Min(3, avaliableItems.Count())).ToList();

			for(int i =0;i<6;i++)
			{
				avaliableItems.Add ( FindObjectOfType<RoomMap> ().LevelSettingsAsset.Cards[UnityEngine.Random.Range(0, FindObjectOfType<RoomMap> ().LevelSettingsAsset.Cards.Count()-1)]);
			}

			float costDifference = FindObjectOfType<RoomMap> ().LevelSettingsAsset.ShopPercentGap;

			for(int i = 0; i<avaliableItems.Count; i++)
			{
				object item = avaliableItems[i];
				int cost = 0;
				if(item.GetType()==typeof(Card))
				{
					cost = ((Card)item).Level;
				}
				if(item.GetType()==typeof(Item))
				{
					cost = ((Item)item).Cost;
				}
				GameObject newItem = Instantiate (ItemPrefab);
				newItem.GetComponent<ItemVisual> ().Init (item, 
					(ItemVisual iv)=>{iv.SetColor(1); ItemHovered(iv);},
					(ItemVisual iv)=>{iv.SetColor(0); ItemUnHovered(iv);},
					(ItemVisual iv)=>{
						GiveItemToPlayer(iv, PhotonNetwork.player);
					},
					Mathf.RoundToInt(cost*(1+UnityEngine.Random.Range(-costDifference, costDifference))));
				
				newItem.transform.SetParent (shopSlots.GetChild(i));
				newItem.transform.localScale = Vector3.one;
				newItem.transform.localPosition = Vector3.zero;
			}
        }

        public void ItemUnHovered(ItemVisual itemVisual)
        {
            ItemName.text = "";
            ItemDescription.text = "";
            cardVisual.gameObject.SetActive(false);
        }

        public void ItemHovered(ItemVisual itemVisual)
        {
            string itemName = "shells";
            string description = "everybody love shells";

            if (itemVisual.item.GetType() == typeof(Card))
            {
                Card c = ((Card)itemVisual.item);
                itemName = "";
                description = "";
                cardVisual.Init(c);
                cardVisual.gameObject.SetActive(true);
            }
            if (itemVisual.item.GetType() == typeof(Item))
            {
                Item i = ((Item)itemVisual.item);
                itemName = i.ItemName;
                description = i.ItemDescription;
            }

            ItemName.text = itemName;
            ItemDescription.text = description;
        }

        public void HidePanel()
        {
			StopAllCoroutines ();
			foreach(ItemVisual iv in GetComponentsInChildren<ItemVisual>())
			{
				Destroy (iv.gameObject);
			}
            Visual.SetActive(false);
        }

        private IEnumerator Tick(float choosingTime)
        {
            float t = 0;
            while (t <= choosingTime)
            {
                t += Time.deltaTime;
                timeSlider.value = timeSlider.maxValue - t;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            HidePanel();
        }

        private void GiveItemToPlayer(ItemVisual item, PhotonPlayer winer)
        {
			if(item.currentCost<=PlayerStats.Instance.PlayerMoney)
			{
				PlayerStats.Instance.PlayerMoney -= item.currentCost;
                item.GiveToPlayer(winer);

				foreach(ItemVisual iv in GetComponentsInChildren<ItemVisual>())
				{
					iv.Init (iv.item, null, null,null, iv.currentCost);
				}

				if(GetComponentsInChildren<ItemVisual>().Length==0)
				{
					HidePanel ();
				}
			}
        }

        public void ReadyClicked()
        {
            HidePanel();
        }
    }
}
