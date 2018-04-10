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
    public class LobbyPlayer : NetworkLobbyPlayer
    {

		public Image colorCircle;
		public Image playerAvatar;
		public TextMeshProUGUI playerNameField;
		public TMP_Dropdown deckDropdown;

        static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
        //used on server to avoid assigning the same color to two player
        static List<int> _colorInUse = new List<int>();

        public Button readyButton;
        public Button waitingPlayerButton;
        public Button removePlayerButton;

        //OnMyName function will be invoked on clients when server change the value of playerName
        [SyncVar(hook = "OnMyName")]
        public string playerName = "";
        [SyncVar(hook = "OnMyColor")]
        public Color playerColor = Color.white;
		[SyncVar(hook = "OnMyAvatar")]
		public int playerSpriteIndex;
        [SyncVar(hook = "OnMyDeck")]
        public string playerDeck;

        private PlayerSaveStruct _player;
        public PlayerSaveStruct Player
        {
            get
            {
                return _player;
            }
        }

        public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        public Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

        static Color JoinColor = new Color(255.0f/255.0f, 0.0f, 101.0f/255.0f,1.0f);
        static Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
        static Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
        static Color TransparentColor = new Color(0, 0, 0, 0);

        //static Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        //static Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

			
	
        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();

            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(1);

            LobbyPlayerList._instance.AddPlayer(this);
            if (isLocalPlayer)
            {
                SetupLocalPlayer();
            }
            else
            {
                SetupOtherPlayer();
            }

            //setup the player data on UI. The value are SyncVar so the player
            //will be created with the right value currently on server
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            //if we return from a game, color of text can still be the one for "Ready"
			readyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

           SetupLocalPlayer();
        }

        void ChangeReadyButtonColor(Color c)
        {
            ColorBlock b = readyButton.colors;
            b.normalColor = c;
            b.pressedColor = c;
            b.highlightedColor = c;
            b.disabledColor = c;
            readyButton.colors = b;
        }

        void SetupOtherPlayer()
        {
			deckDropdown.gameObject.SetActive (false);
			transform.SetAsLastSibling ();
			removePlayerButton.gameObject.SetActive(NetworkServer.active);
            ChangeReadyButtonColor(NotReadyColor);
			readyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "...";
            readyButton.interactable = false;
            OnClientReady(false);
			OnMyName(playerName);
			OnMyColor(playerColor);
			OnMyAvatar (playerSpriteIndex);
        }

        void SetupLocalPlayer()
        {
            _player = LobbyPlayerIdentity.Instance.player;

            deckDropdown.gameObject.SetActive (true);
			deckDropdown.ClearOptions ();
			deckDropdown.AddOptions (LobbyPlayerIdentity.Instance.player.Decks.Where(d=>d.Awaliable).Select(d=>d.DeckName).ToList());

            if (!LobbyPlayerIdentity.Instance.player.Decks.Where(d => d.Awaliable).ToList().Contains(LobbyPlayerIdentity.Instance.player.CurrentDeck))
            {
                LobbyPlayerIdentity.Instance.player.CurrentDeck = LobbyPlayerIdentity.Instance.player.Decks.Where(d => d.Awaliable).ToList()[deckDropdown.value];
            }
            else
            {
                deckDropdown.value = LobbyPlayerIdentity.Instance.player.Decks.Where(d => d.Awaliable).ToList().IndexOf(LobbyPlayerIdentity.Instance.player.CurrentDeck);
            }

            string cards = "";
            foreach (Card c in LobbyPlayerIdentity.Instance.player.CurrentDeck.Cards)
            {
                cards += DefaultResourcesManager.AllCards.ToList().IndexOf(c) + ",";
            }
            CmdDeckChange(cards);

            deckDropdown.onValueChanged.AddListener(DeckDropdownChanged);
            transform.SetAsFirstSibling ();
            CheckRemoveButton();
			CmdNameChanged (LobbyPlayerIdentity.Instance.player.PlayerName);
			CmdColorChange(LobbyPlayerIdentity.Instance.player.PlayerColor);
			CmdAvatarChange(DefaultResourcesManager.Avatars.ToList().IndexOf(LobbyPlayerIdentity.Instance.player.PlayerAvatar));

            ChangeReadyButtonColor(JoinColor);

			removePlayerButton.gameObject.SetActive (true);
			readyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Ready";
            readyButton.interactable = true;

            //we switch from simple name display to name input
          
            readyButton.onClick.RemoveAllListeners();
            readyButton.onClick.AddListener(OnReadyClicked);

            //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
            //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(0);

			playerNameField.text = LobbyPlayerIdentity.Instance.player.PlayerName;
			colorCircle.color = LobbyPlayerIdentity.Instance.player.PlayerColor;
			playerAvatar.sprite = LobbyPlayerIdentity.Instance.player.PlayerAvatar;

        }

        private void DeckDropdownChanged(int v)
        {
            LobbyPlayerIdentity.Instance.player.CurrentDeck = LobbyPlayerIdentity.Instance.player.Decks.Where(d => d.Awaliable).ToList()[v];
            string cards = "";
            foreach (Card c in LobbyPlayerIdentity.Instance.player.CurrentDeck.Cards)
            {
                cards += DefaultResourcesManager.AllCards.ToList().IndexOf(c) + ",";
            }
            CmdDeckChange(cards);
        }

        //This enable/disable the remove button depending on if that is the only local player or not
        public void CheckRemoveButton()
        {
            if (!isLocalPlayer)
                return;

            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

        }

        public override void OnClientReady(bool readyState)
        {
            if (readyState)
            {
                ChangeReadyButtonColor(TransparentColor);

				TextMeshProUGUI textComponent = readyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                textComponent.text = "READY";
                textComponent.color = ReadyColor;
                readyButton.interactable = false;
               
            }
            else
            {
                ChangeReadyButtonColor(isLocalPlayer ? JoinColor : NotReadyColor);

				TextMeshProUGUI textComponent = readyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                textComponent.text = isLocalPlayer ? "Ready" : "...";
                textComponent.color = Color.white;
                readyButton.interactable = isLocalPlayer;
            }
        }

        public void OnPlayerListChanged(int idx)
        { 
            GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
        }

        ///===== callback from sync var

        private void OnMyDeck(string deck)
        {
            playerDeck = deck;
        }

        public void OnMyName(string newName)
        {
            playerName = newName;
			playerNameField.text = playerName;
        }

        public void OnMyColor(Color newColor)
        {
            playerColor = newColor;
			colorCircle.color = newColor;
        }

		public void OnMyAvatar(int newSpriteIndex)
		{
			playerSpriteIndex = newSpriteIndex;
			playerAvatar.sprite = DefaultResourcesManager.Avatars[newSpriteIndex];
		}

        //===== UI Handler

        //Note that those handler use Command function, as we need to change the value on the server not locally
        //so that all client get the new value throught syncvar
       

        public void OnReadyClicked()
        {
            SendReadyToBeginMessage();
        }

        public void OnNameChanged(string str)
        {
            CmdNameChanged(str);
        }

        public void OnRemovePlayerClick()
        {
            if (isLocalPlayer)
            {
				foreach(LobbyPlayer lp in FindObjectsOfType<LobbyPlayer>())
				{
					LobbyManager.s_Singleton.RemovePlayer (lp);
				}
				LobbyManager.s_Singleton.StopHostClbk();
            }
            else if (isServer)
                LobbyManager.s_Singleton.KickPlayer(connectionToClient);
                
        }



        public void ToggleJoinButton(bool enabled)
        {
            readyButton.gameObject.SetActive(enabled);
            waitingPlayerButton.gameObject.SetActive(!enabled);
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            LobbyManager.s_Singleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
            LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
        }

        [ClientRpc]
        public void RpcUpdateRemoveButton()
        {
            CheckRemoveButton();
        }

        //====== Server Command

        [Command]
		public void CmdColorChange(Color color)
        {
			playerColor = color;
        }

        [Command]
        public void CmdDeckChange(string deck)
        {
            playerDeck = deck;
        }

        [Command]
		public void CmdAvatarChange(int avatar)
		{
			playerSpriteIndex = avatar;
		}


        [Command]
        public void CmdNameChanged(string name)
        {
            playerName = name;
        }

        //Cleanup thing when get destroy (which happen when client kick or disconnect)
        public void OnDestroy()
        {
            LobbyPlayerList._instance.RemovePlayer(this);
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(-1);

            int idx = System.Array.IndexOf(Colors, playerColor);

            if (idx < 0)
                return;

            for (int i = 0; i < _colorInUse.Count; ++i)
            {
                if (_colorInUse[i] == idx)
                {//that color is already in use
                    _colorInUse.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
