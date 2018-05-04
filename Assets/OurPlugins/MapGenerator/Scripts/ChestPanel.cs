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
            timeSlider.maxValue = choosingTime;
            timeSlider.value = choosingTime;
            Visual.SetActive(true);
            CreateItems();
            StartCoroutine(Tick(choosingTime));
        }

        public void HidePanel()
        {
            //give items to players

            Dictionary<PhotonPlayer, float> playersWeights = new Dictionary<PhotonPlayer, float>();

            Dictionary<PhotonPlayer, List<ItemVisual>> playersItems = new Dictionary<PhotonPlayer, List<ItemVisual>>();


            foreach (PhotonPlayer pp in FindObjectOfType<RoomMap>().Players)
            {
                    playersItems.Add(pp, new List<ItemVisual>());
            }

            foreach (ItemVisual iv in GetComponentsInChildren<ItemVisual>())
            {
                foreach (PhotonPlayer pp in iv.players)
                {
                    if (playersWeights.ContainsKey(pp))
                    {
                        playersWeights[pp] += 1;
                    }
                    else
                    {
                        playersWeights.Add(pp, 1);
                    }
                }  
            }

            foreach (ItemVisual iv in GetComponentsInChildren<ItemVisual>())
            {
                Dictionary<PhotonPlayer, float> playerChances = new Dictionary<PhotonPlayer, float>();
                float globalChance = 0;
                foreach (PhotonPlayer pp in iv.players)
                {
                    globalChance += 1f / playersWeights[pp];
                    playerChances.Add(pp, 1f/playersWeights[pp]);
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

            StopCoroutine(Tick(choosingTime));
            Invoke("HideAllPanel", 1.5f);
        }

        private void HideAllPanel()
        {
            Visual.SetActive(false);
        }

        private void GiveItemToPlayer(List<ItemVisual> items, PhotonPlayer winer)
        {
            Debug.Log(items.Count);
            foreach (ItemVisual iv in items)
            {
                iv.GiveToPlayer(winer);
            }
        }

        private void CreateItems()
        {
            List<Item> avaliableItems = FindObjectOfType<RoomMap>().LevelSettingsAsset.Items;

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
            value = Mathf.Clamp(value - item.level, 0, value);

            List<Card> cards = FindObjectOfType<RoomMap>().LevelSettingsAsset.Cards;

            List<Card> choosedCards = new List<Card>();

            for (int i = 0; i < 3; i++)
            {
                choosedCards.Add(cards.OrderBy(c => Mathf.Abs(c.Level - cardsValues[i])).First());
            }

            float money = 1f - value / 3 - cardsValues[0] - cardsValues[1] - cardsValues[2];
            money = Mathf.Clamp(money, UnityEngine.Random.Range(5,10), money);

            List<GameObject> createdItems = new List<GameObject>();

            GameObject go = Instantiate(ItemPrefab);
            go.GetComponent<ItemVisual>().Init(item);
            createdItems.Add(go);

            GameObject go2 = Instantiate(ItemPrefab);
            go2.GetComponent<ItemVisual>().Init(Mathf.RoundToInt(money));
            createdItems.Add(go2);

            GameObject go3 = Instantiate(ItemPrefab);
            go3.GetComponent<ItemVisual>().Init(choosedCards[0]);
            createdItems.Add(go3);

            GameObject go4 = Instantiate(ItemPrefab);
            go4.GetComponent<ItemVisual>().Init(choosedCards[1]);
            createdItems.Add(go4);

            GameObject go5 = Instantiate(ItemPrefab);
            go5.GetComponent<ItemVisual>().Init(choosedCards[2]);
            createdItems.Add(go5);

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
