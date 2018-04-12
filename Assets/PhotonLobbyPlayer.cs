using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System;

namespace Prototype.NetworkLobby
{
    //Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
    //Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
    public class PhotonLobbyPlayer : Photon.MonoBehaviour
    {

        public Image colorCircle;
        public Image playerAvatar;
        public TextMeshProUGUI playerNameField;
        public TMP_Dropdown deckDropdown;
        public Button readyButton;
        public Button waitingPlayerButton;
        public Button removePlayerButton;
        private PhotonPlayer owner;

        public void ChangeServer()
        {
            if (owner != PhotonNetwork.player)
            {
                removePlayerButton.gameObject.SetActive(true);
                removePlayerButton.onClick.RemoveAllListeners();
                removePlayerButton.onClick.AddListener(KickPlayer);
            }
        }

        public void Init(string name, float[] color, int spriteId, PhotonPlayer player)
        {
            owner = player;
            colorCircle.color = new Color(color[0], color[1], color[2]);
            playerAvatar.sprite = DefaultResourcesManager.Avatars[spriteId];
            playerNameField.text = name;
            if (player!=PhotonNetwork.player)
            {
                deckDropdown.gameObject.SetActive(false);
            }
            else
            {
                deckDropdown.gameObject.SetActive(true);
                deckDropdown.ClearOptions();
                deckDropdown.AddOptions(LobbyPlayerIdentity.Instance.player.Decks.Where(d => d.Awaliable).Select(d => d.DeckName).ToList());
                if (!LobbyPlayerIdentity.Instance.player.Decks.Where(d => d.Awaliable).ToList().Contains(LobbyPlayerIdentity.Instance.player.CurrentDeck))
                {
                    LobbyPlayerIdentity.Instance.player.CurrentDeck = LobbyPlayerIdentity.Instance.player.Decks.Where(d => d.Awaliable).ToList()[deckDropdown.value];
                }
                else
                {
                    deckDropdown.value = LobbyPlayerIdentity.Instance.player.Decks.Where(d => d.Awaliable).ToList().IndexOf(LobbyPlayerIdentity.Instance.player.CurrentDeck);
                }
            }

            deckDropdown.onValueChanged.AddListener(DeckDropdownChanged);

            if (PhotonNetwork.isMasterClient)
            {
                removePlayerButton.gameObject.SetActive(true);
                if (player == PhotonNetwork.player)
                {
                    //quit and change host
                    removePlayerButton.onClick.AddListener(QuitRoomAsHost);
                }
                else
                {
                    //kick
                    removePlayerButton.onClick.AddListener(KickPlayer);
                }
            }
            else
            {
                //quit room
                removePlayerButton.gameObject.SetActive(player == PhotonNetwork.player);
                removePlayerButton.onClick.AddListener(QuitRoom);
            }
        }

        private void KickPlayer()
        {
            GetComponent<PhotonView>().RPC("Kicked", PhotonNetwork.player, new object[0]);
        }

        [PunRPC]
        private void Kicked()
        {
            QuitRoom();
            GetComponent<PhotonView>().RPC("PlyerQuit", PhotonTargets.OthersBuffered, new object[1] {PhotonNetwork.player});
        }

        [PunRPC]
        private void PlyerQuit(PhotonPlayer player)
        {
            foreach (PhotonLobbyPlayer plp in FindObjectsOfType<PhotonLobbyPlayer>())
            {
                if (player == plp.owner)
                {
                    Destroy(plp.gameObject);
                }
            }
        }

        private void QuitRoom()
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("QuitRoom");
            FindObjectOfType<GameLauncher>().OnDisconnectedFromPhoton();
            foreach (PhotonLobbyPlayer plp in FindObjectsOfType<PhotonLobbyPlayer>())
            {
                Destroy(plp.gameObject);
            }
            LobbyMenu.Instance.BackToMainMenu();
        }

        private void QuitRoomAsHost()
        {
            foreach (PhotonLobbyPlayer plp in FindObjectsOfType<PhotonLobbyPlayer>())
            {
                if (plp!=this)
                {
                    PhotonNetwork.SetMasterClient(plp.owner);
                    break;
                }    
            }

            QuitRoom();
        }

        private void DeckDropdownChanged(int v)
        {
            LobbyPlayerIdentity.Instance.player.CurrentDeck = LobbyPlayerIdentity.Instance.player.Decks.Where(d => d.Awaliable).ToList()[v];
            string cards = "";
            foreach (Card c in LobbyPlayerIdentity.Instance.player.CurrentDeck.Cards)
            {
                cards += DefaultResourcesManager.AllCards.ToList().IndexOf(c) + ",";
            }
        }
    }
}
