using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LobbyPlayerIdentity : Singleton<LobbyPlayerIdentity> 
{
	public PlayerSaveStruct player;

	private void Start()
	{
        string saveFolderPath = Path.Combine(Application.persistentDataPath, "Player");
        string savePath = Path.Combine(saveFolderPath, "playerSave");

        if (!File.Exists(savePath))
        {
            InitPlayerDefault();
        }
        else
        {
            player = JsonUtility.FromJson<PlayerSaveStruct>(File.ReadAllText(savePath));
        }
		DontDestroyOnLoad (this);
	}

	private void InitPlayerDefault()
	{
		player.PlayerName = DefaultResourcesManager.GetRandomName();
		player.PlayerColor = DefaultResourcesManager.GetRandomColor();
		player.PlayerAvatarId = DefaultResourcesManager.GetRandomAvatar ();
        player.Decks = new List<DeckStruct>();

        DeckStruct startingDeck =  DefaultResourcesManager.StartingDeck;
        startingDeck = new DeckStruct(startingDeck.DeckName, startingDeck.Cards);
        player.Decks.Add(startingDeck);

        for (int i = 0; i < 3; i++) 
		{
			foreach(Card c in DefaultResourcesManager.AllCards)
			{
                player.AllCards.Add(c.name);
            }		
		}
	}

    void OnApplicationQuit()
    {
        string json = JsonUtility.ToJson(player);

        string saveFolderPath = Path.Combine(Application.persistentDataPath, "Player");

		Debug.Log (saveFolderPath);

        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
        string savePath = Path.Combine(saveFolderPath, "playerSave");   
        if (!File.Exists(savePath))
        {
            FileStream fs = File.Create(savePath);
            fs.Close();
        }
        File.WriteAllText(savePath, json);
    }
}
