using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WarriorObject : MonoBehaviour {

	public HpSlider hpSlider;
	public TextMeshProUGUI warriorName;

	private WarriorVisual warriorVisual;

	public bool IsEnemy
	{
		get
		{
			return Player == null;
		}
	}
	private Warrior warriorAsset;
	public Warrior WarriorAsset
	{
		get
		{
			return warriorAsset;
		}
	}
	private PhotonPlayer player;
	public PhotonPlayer Player
	{
		get
		{
			return player;
		}
	}
		

	public void Init(Warrior warrior, PhotonPlayer player)
	{
		this.player = player;
		this.warriorAsset = warrior;
		hpSlider.Init(warrior.hp);
		warriorName.text = warrior.WarriorName;
		warriorVisual = Instantiate (warrior.visual).GetComponent<WarriorVisual>();
		warriorVisual.transform.SetParent (transform);
		warriorVisual.transform.SetAsFirstSibling ();
		warriorVisual.transform.localPosition = Vector3.zero;
		warriorVisual.transform.localScale = Vector3.one;
	}

	public void RecieveDamage(int dmg)
	{
		hpSlider.Hp += dmg;
	}

	public void Animate()
	{
		warriorVisual.Animate (WarriorVisual.SkillAnimation.Atack);
	}

}
