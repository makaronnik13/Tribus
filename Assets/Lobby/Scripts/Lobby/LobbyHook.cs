using UnityEngine;
using UnityEngine.Networking;
using System.Collections;



namespace Prototype.NetworkLobby
{
    // Subclass this and redefine the function you want
    // then add it to the lobby prefab
    public class LobbyHook : MonoBehaviour
    {
        public void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) 
		{
            Debug.Log(lobbyPlayer.GetComponent<LobbyPlayer>().Player.CurrentDeck);
            PlayerSaveStruct player = lobbyPlayer.GetComponent<LobbyPlayer>().Player;
            gamePlayer.GetComponent<GamePlayerIdentity>().player = new Player(player.PlayerName, player.PlayerColor, player.PlayerAvatar, player.CurrentDeck.Cards);
        }
    }
}
