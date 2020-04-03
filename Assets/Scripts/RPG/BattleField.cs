using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class BattleField : Singleton<BattleField> {

	public Warrior[] enemies;
	public GameObject WarriorPrefab;

	private Transform playersTransform;
	private Transform enemiesTransform;

	public List<WarriorObject> Enemies = new List<WarriorObject>();
	public List<WarriorObject> Players = new List<WarriorObject>();

	#region LifeCycle
	void Awake()
	{
		playersTransform = transform.GetChild (0);
		enemiesTransform = transform.GetChild (1);
	}
	#endregion


	public void StartBattle(List<PlayerVisual> visuals)
	{
		List<Warrior> warriors = new List<Warrior> ();
		foreach(PlayerVisual pv in visuals)
		{
			warriors.Add (pv.Warrior);
		}
		StartBattle (warriors.ToArray(), enemies);
	}

	private void StartBattle(Warrior[] players, Warrior[] enemies)
	{
		List<WarriorObject> warriors = new List<WarriorObject> ();

		for(int i = 0; i<players.Length; i++)
		{
			GameObject w = Instantiate (WarriorPrefab);
			w.GetComponent<WarriorObject> ().Init (players[i], PhotonNetwork.player);
			w.transform.SetParent (playersTransform.GetChild(i));
			w.transform.localPosition = Vector3.zero;
			w.transform.localScale = Vector3Int.one;
			warriors.Add (w.GetComponent<WarriorObject> ());
			Players.Add (w.GetComponent<WarriorObject>());
		}

		for(int i = 0; i<enemies.Length; i++)
		{
			GameObject w = Instantiate (WarriorPrefab);
            w.GetComponent<WarriorObject> ().Init (enemies[i], null);
			w.transform.SetParent (enemiesTransform.GetChild(i));
			w.transform.localPosition = Vector3.zero;
			w.transform.localScale = Vector3Int.one;
			warriors.Add (w.GetComponent<WarriorObject> ());
			Enemies.Add (w.GetComponent<WarriorObject>());
		}



		InitiativeTimeline.Instance.StartBattle (warriors.ToArray());
	}

	public void GiveTurn(WarriorObject warrior)
	{
		if (warrior.IsEnemy)
        {
			AIController.Instance.PerformAction (warrior);
		} else 
		{
			RPGCardGameManager.sInstance.StartPlayerTurn (warrior.Player);
		}
	}
}
