using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

public class WarriorObject : MonoBehaviour
{
	public HpSlider hpSlider;
    public BlockVisual blockVisual;
	public TextMeshProUGUI warriorName;
	public Transform modifiersHab;

	private GameObject _hpParticle;
	private GameObject hpParticle
	{
		get
		{
			if(!_hpParticle)
			{
				_hpParticle = Resources.Load ("Prefabs/RPG/HpParticle") as GameObject;
			}
			return _hpParticle;
		}
	}

	private List<EfectVisual> effects = new List<EfectVisual> ();

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
        blockVisual.Init();
		warriorName.text = warrior.WarriorName;
		warriorVisual = Instantiate (warrior.visual).GetComponentInChildren<WarriorVisual>();
		warriorVisual.transform.parent.SetParent (transform);
		warriorVisual.transform.parent.SetAsFirstSibling ();
		warriorVisual.transform.parent.localPosition = Vector3.zero;
		warriorVisual.transform.parent.localScale = Vector3.one;
		InitiativeTimeline.Instance.OnTick += OnTick;
	}

    public void AddModifier(Effect addingEffect, float time)
    {
		EfectVisual ev = effects.FirstOrDefault (e=>e.Modifier == addingEffect);
		if (ev)
        {
			ev.Init(addingEffect, ev.StayTime+time);
        }
        else
        {
			ev = Instantiate (Resources.Load("Prefabs/RPG/Modifier") as GameObject).GetComponent<EfectVisual>();
			ev.transform.SetParent (modifiersHab);
			ev.transform.localScale = Vector3.one;
			ev.transform.localPosition = Vector3.zero;
			ev.Init (addingEffect, time);
			effects.Add (ev);
        }
    }

    public void GetBlock(int value)
    {
        blockVisual.Block += value;
    }

    public void RecieveDamage(int dmg)
	{
        int damageToHp = dmg;
        if (blockVisual.Block>0 && dmg>0)
        {
            damageToHp -= blockVisual.Block;
            GetBlock(damageToHp-dmg);
            damageToHp = Mathf.Clamp(damageToHp, 0, damageToHp);
        }

		hpSlider.Hp -= damageToHp;

        if (dmg > 0)
        {
            Animate(null, WarriorVisual.SkillAnimation.Hit);
        }
        else
        {
            Animate(null, WarriorVisual.SkillAnimation.Heal);
        }

        if (hpSlider.Hp <=0)
        {
            Die();
        }
	}

    private void Die()
    {
		InitiativeTimeline.Instance.OnTick -= OnTick;
        Animate(()=> {
            GetComponentInChildren<Canvas>().gameObject.SetActive(false);
            InitiativeTimeline.Instance.RemoveWarrior(this);
        }, WarriorVisual.SkillAnimation.Die);
        
    }

    public void Animate(Action action = null, WarriorVisual.SkillAnimation animation = WarriorVisual.SkillAnimation.Atack)
	{
		warriorVisual.Animate (animation, action);
	}

	void OnTick()
	{
		for(int i = effects.Count()-1; i>=0; i--)
		{
			EfectVisual ev = effects [i];
			ev.Activate ();
			AddModifier (ev.Modifier, -1);
			if(ev.StayTime<=0)
			{
				effects.Remove (ev);
				Destroy (ev.gameObject);
			}
		}
	}

	public void EmmitParticle(int dmg, bool toBlock = false)
	{
		GameObject chh = Instantiate (hpParticle) as GameObject;
		chh.transform.SetParent (GetComponentInChildren<Canvas>().transform);
		chh.transform.localScale = Vector3.one;
		chh.transform.localPosition = Vector3.zero;
		chh.GetComponent<ChangeHpPartice> ().Init (-dmg, toBlock);
	}
		
}
