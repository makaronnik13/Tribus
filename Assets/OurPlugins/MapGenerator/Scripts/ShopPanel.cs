using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            StopCoroutine(Tick(choosingTime));
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

        private void GiveItemToPlayer(List<ItemVisual> items, PhotonPlayer winer)
        {
            Debug.Log(items.Count);
            foreach (ItemVisual iv in items)
            {
                iv.GiveToPlayer(winer);
            }
        }

        public void ReadyClicked()
        {
            HidePanel();
        }
    }
}
