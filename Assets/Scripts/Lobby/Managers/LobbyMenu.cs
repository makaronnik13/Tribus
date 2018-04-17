using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using ExitGames.Demos.DemoAnimator;

public class LobbyMenu : Singleton<LobbyMenu> {

	public GameObject DeckPanel, PlayerPanel, MainMenuPanel, LobbyPanel, RoomPanel, SandboxPanel, Background;
    public GameLauncher launcher;

	void Start()
	{
		DontDestroyOnLoad (this);
	}

	public void Quit()
	{
		Application.Quit ();
	}

	public void Play()
	{
		launcher.Connect();
		MainMenuPanel.SetActive (false);
        RoomPanel.SetActive(true);
    }

	public void PlayerSettings()
	{
		MainMenuPanel.SetActive (false);
		PlayerPanel.SetActive (true);
	}

	public void BackToPlayerSetings()
	{
		PlayerPanel.SetActive (true);
		DeckPanel.SetActive (false);
	}

	public void EditDeck(DeckStruct ds)
	{
		PlayerPanel.SetActive (false);
		DeckPanel.SetActive (true);
		DeckPanel.GetComponent<DeckEditPanel> ().Edit (ds);
	}

	public void BackToMainMenu()
	{
		LobbyPanel.SetActive (false);
		DeckPanel.SetActive (false);
		PlayerPanel.SetActive (false);
		MainMenuPanel.SetActive (true);
		SandboxPanel.SetActive (false);
	}


	public void ExitLobby()
	{
		//RoomPanel.SetActive (false);
		Play ();
	}

	public void EnterLobby()
	{
		LobbyPanel.SetActive (false);
		//RoomPanel.SetActive (true);
	}

	public void Sandbox()
	{
		MainMenuPanel.SetActive (false);
		SandboxPanel.SetActive (true);
	}

    public void HideBackGround()
    {
        Background.SetActive(false);
    }
}
