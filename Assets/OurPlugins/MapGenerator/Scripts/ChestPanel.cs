using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine.UI;

namespace Tribus
{
    public class ChestPanel : MonoBehaviour
    {
        public GameObject ItemPrefab;
        public GameObject Visual;
        public ChestVisual chestVisual;
        public TextMeshProUGUI ItemName;
        public TextMeshProUGUI ItemDescription;
        public CardVisual cardVisual;
        public Slider timeSlider;

        public float choosingTime = 30;

        public int testLevel;

		private  Dictionary<PhotonPlayer, List<ItemVisual>> playersItems = new Dictionary<PhotonPlayer, List<ItemVisual>>();

        // Use this for initialization
        void Start()
        {
            FindObjectOfType<RoomActivator>().OnChestIn += ShowPanel;
        }

        [Button("test")]
        public void Test()
        {
            FindObjectOfType<PlayerStats>().CurrentPartyLevel = testLevel;
            ShowPanel();
        }


        public void ItemHovered(ItemVisual itemVisual)
        {
            string itemName = "shells";
            string description = "everybody love shells";

            if(itemVisual.item.GetType() == typeof(Card))
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

        public void ItemUnHovered(ItemVisual itemVisual)
        {
            ItemName.text = "";
            ItemDescription.text = "";
            cardVisual.gameObject.SetActive(false);
        }

        private void ShowPanel()
        {
			foreach(PhotonPlayer pp in FindObjectOfType<RoomMap>().Players)
			{
				playersItems.Add (pp, new List<ItemVisual>());
			}
            timeSlider.maxValue = choosingTime;
            timeSlider.value = choosingTime;
            Visual.SetActive(true);
            CreateItems();
            StartCoroutine(Tick(choosingTime));
        }

        public void HidePanel()
        {
            //give items to players

            foreach (ItemVisual iv in GetComponentsInChildren<ItemVisual>())
            {
                Dictionary<PhotonPlayer, float> playerChances = new Dictionary<PhotonPlayer, float>();
                float globalChance = 0;
				foreach (KeyValuePair<PhotonPlayer, List<ItemVisual>> pair in playersItems)
                {
					if(pair.Value.Contains(iv))
					{
						globalChance += 1f / pair.Value.Count();
						playerChances.Add(pair.Key, 1f/pair.Value.Count());
					}
                }

                if (playerChances.Count > 0)
                {
                    float probability = UnityEngine.Random.Range(0, globalChance);
                    PhotonPlayer winer = playerChances.SkipWhile(i => i.Value < probability).First().Key;
                    playersItems[winer].Add(iv);
                }
            }

            foreach (ItemVisual iv in GetComponentsInChildren<ItemVisual>())
            {
                bool itemChoosed = false;
                foreach (KeyValuePair<PhotonPlayer, List<ItemVisual>> pair in playersItems)
                {
                    if (pair.Value.Contains(iv))
                    {
                        itemChoosed = true;
                    }
                }

                if (!itemChoosed)
                {
                        PhotonPlayer winer = playersItems.OrderByDescending(k => k.Value.Count).First().Key;
                        playersItems[winer].Add(iv);
                }
            }

            foreach (KeyValuePair<PhotonPlayer, List<ItemVisual>> pair in playersItems)
            {
                GiveItemToPlayer(pair.Value, pair.Key);
            }

			StopAllCoroutines ();
            Invoke("HideAllPanel", 1.5f);
			playersItems.Clear ();
        }

        private void HideAllPanel()
        {
            Visual.SetActive(false);
        }

        private void GiveItemToPlayer(List<ItemVisual> items, PhotonPlayer winer)
        {
            foreach (ItemVisual iv in items)
            {
                iv.GiveToPlayer(winer);
            }
        }

        private void CreateItems()
        {
			List<Item> avaliableItems = new List<Item>(FindObjectOfType<RoomMap>().LevelSettingsAsset.Items);
			List<object> items = new List<object>();

            //temp fake. collect items from all players
            foreach (PlayerStats ps in FindObjectsOfType<PlayerStats>())
            {
                foreach (Item i in ps.PlayerItems)
                {
                    avaliableItems.Remove(i);
                }
            }

            int value = FindObjectOfType<PlayerStats>().CurrentPartyLevel*10;
            float percentGap = FindObjectOfType<RoomMap>().LevelSettingsAsset.PercentGap;
            value = Mathf.CeilToInt(value * (1 + UnityEngine.Random.Range(-percentGap, percentGap)));

            float itemValue = value / 3;
            float[] cardsValues = new float[3]{
                value/UnityEngine.Random.Range(3f,15f),
                value/UnityEngine.Random.Range(3f,15f),
                value/UnityEngine.Random.Range(3f,15f)
            };
            

            Item item = avaliableItems.OrderBy(i => Mathf.Abs(i.level - itemValue)).First();
			items.Add (item);

            List<Card> cards = FindObjectOfType<RoomMap>().LevelSettingsAsset.Cards;

            List<Card> choosedCards = new List<Card>();

            for (int i = 0; i < 3; i++)
            {
				items.Add(cards.OrderBy(c => Mathf.Abs(c.Level - cardsValues[i])).First());
            }

			int money = Mathf.RoundToInt(value - itemValue - cardsValues[0] - cardsValues[1] - cardsValues[2]);
            money = Mathf.Clamp(money, UnityEngine.Random.Range(5,10), money);

			items.Add (money);

            List<GameObject> createdItems = new List<GameObject>();

			foreach(object ite in items)
			{
				GameObject go = Instantiate(ItemPrefab);
				go.GetComponent<ItemVisual>().Init(ite, 
					(ItemVisual iv)=>{ItemHovered(iv);iv.SetColor(1);},
					(ItemVisual iv)=>{
						ItemUnHovered(iv);
						if (!playersItems[PhotonNetwork.player].Contains (iv)) 
						{
							iv.SetColor (0);
						} else 
						{
							iv.SetColor(2);
						}	
					},
					(ItemVisual iv)=>{
						if(!playersItems[PhotonNetwork.player].Contains(iv.item))
						{
							iv.SetColor(2);
							playersItems[PhotonNetwork.player].Add(iv);
						}
						else
						{
							playersItems[PhotonNetwork.player].Remove(iv);
							iv.SetColor(1);
						}
					});
				createdItems.Add(go);
			}
            chestVisual.ShowItems(createdItems);
        }

        private IEnumerator Tick(float choosingTime)
        {
            float t = 0;
            while (t<=choosingTime)
            {
                t += Time.deltaTime;
                timeSlider.value = timeSlider.maxValue-t;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            HidePanel();
        }

        public void ReadyClicked()
        {
            HidePanel();
        }
    }
}
