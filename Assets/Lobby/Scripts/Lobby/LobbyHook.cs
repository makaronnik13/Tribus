using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.NetworkLobby
{
    // Subclass this and redefine the function you want
    // then add it to the lobby prefab
    //hook calls on server

    public class LobbyHook : MonoBehaviour
    {
        public void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) 
		{
            LobbyPlayer player = lobbyPlayer.GetComponent<LobbyPlayer>();

            List<Card> deck = new List<Card>();
            foreach (string cardId in lobbyPlayer.GetComponent<LobbyPlayer>().playerDeck.Split(new char[] {','}))
            {
                if (cardId!="")
                {
                    deck.Add(DefaultResourcesManager.AllCards[int.Parse(cardId)]);
                }
            }
            gamePlayer.GetComponent<GamePlayer>().player = new Player(player.playerName, player.playerColor, DefaultResourcesManager.Avatars[player.playerSpriteIndex], deck);
        }
    }
}
