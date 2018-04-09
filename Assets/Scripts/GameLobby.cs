using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameLobby : Singleton<GameLobby> {

	public PlayersVisualizer playersVisualiser;
	public List<Player> players;
	private Queue<Player> playersQueue;
	private Player currentPlayer;
	public Player CurrentPlayer
	{
		get{
			return currentPlayer;
		}
		set{
			if(currentPlayer!=value)
			{
				if (currentPlayer!=null) {
					ResourcesManager.Instance.EndTurn ();
					playersQueue.Enqueue (currentPlayer);
					CardsManager.Instance.EndPlayerTurn (currentPlayer);
				}
				currentPlayer = value;
				CounterPanel.Instance.RunCounter (currentPlayer, 5, () => {
					StartTurn (currentPlayer);
				});
			}
		}
	}

	void Start()
	{
		//InitLobby (players);
	}



	public void InitLobby(List<Player> players)
	{
		foreach(Player p in players)
		{
			p.InitPlayer ();
		}
		playersQueue = new Queue<Player> (players.OrderBy(a => Guid.NewGuid()).ToList());
		playersVisualiser.Init (playersQueue.ToList());
		EndTurn ();
	}

	public void EndTurn()
	{
		CurrentPlayer = playersQueue.Dequeue ();
	}

	private void StartTurn(Player player)
	{
		playersVisualiser.SetActivePlayer();
		FakeController.Instance.StartTurn (player);
	}
}
