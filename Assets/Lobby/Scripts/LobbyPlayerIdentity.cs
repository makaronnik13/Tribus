using System.Collections;
using System.Collections.Generic;
using System.IO;
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
		player.Decks = new List<DeckStruct> (){ DefaultResourcesManager.StartingDeck};
        player.CurrentDeck = player.Decks[0];

        Debug.Log(DefaultResourcesManager.AllCards.Length);

        for (int i = 0; i < 3; i++) 
		{
            for (int j = 0; j< DefaultResourcesManager.AllCards.Length;j++)
            {
                player.AllCards.Add(j);
            }		
		}
	}

    void OnApplicationQuit()
    {
        string json = JsonUtility.ToJson(player);

        string saveFolderPath = Path.Combine(Application.persistentDataPath, "Player");

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
