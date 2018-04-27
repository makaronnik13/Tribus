using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Singleton<AIController>
{
	public void PerformAction(WarriorObject warrior)
	{
		WarriorObject aim = BattleField.Instance.Players[Random.Range(0,  BattleField.Instance.Players.Count-1)];

		aim.RecieveDamage (warrior.WarriorAsset.damage);

		warrior.Animate();
	}
}
