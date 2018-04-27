using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

public class WarriorObject : MonoBehaviour, ISkillAim
{
	public HpSlider hpSlider;
    public BlockVisual blockVisual;
	public TextMeshProUGUI warriorName;
    public SpriteRenderer selector;

    private Dictionary<Effect, float> modifiers = new Dictionary<Effect, float>();

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
        blockVisual.Init(warrior.hp);
		warriorName.text = warrior.WarriorName;
		warriorVisual = Instantiate (warrior.visual).GetComponentInChildren<WarriorVisual>();
		warriorVisual.transform.parent.SetParent (transform);
		warriorVisual.transform.parent.SetAsFirstSibling ();
		warriorVisual.transform.parent.localPosition = Vector3.zero;
		warriorVisual.transform.parent.localScale = Vector3.one;
	}

    public void AddModifier(Effect addingEffect, float time)
    {
        if (modifiers.ContainsKey(addingEffect))
        {
            modifiers[addingEffect]+=time;
        }
        else
        {
            modifiers.Add(addingEffect, time);
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
		hpSlider.Hp -= dmg;

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
        Animate(()=> {
            GetComponentInChildren<Canvas>().gameObject.SetActive(false);
            InitiativeTimeline.Instance.RemoveWarrior(this);
        }, WarriorVisual.SkillAnimation.Die);
        
    }

    public void Animate(Action action = null, WarriorVisual.SkillAnimation animation = WarriorVisual.SkillAnimation.Atack)
	{
		warriorVisual.Animate (animation, action);
	}

    public bool IsAwaliable(Card card)
    {
        if (card)
        {
            CardEffect cardEffect = card.CardEffects.FirstOrDefault(ce => ce.cardAim != CardEffect.CardAim.None);
            if (cardEffect != null)
            {
                if (cardEffect.cardAim == CardEffect.CardAim.All)
                {
                    return true;
                }

                if (cardEffect.cardAim == CardEffect.CardAim.Any)
                {
                    return true;
                }

                if (cardEffect.cardAim == CardEffect.CardAim.Enemies && Player == null)
                {
                    return true;
                }

                if (cardEffect.cardAim == CardEffect.CardAim.Enemy && Player == null)
                {
                    return true;
                }

                if (cardEffect.cardAim == CardEffect.CardAim.You && Player == RPGCardGameManager.sInstance.CurrentPlayer.photonPlayer)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Highlight(Card card, bool v)
    {
        if (v)
        {
            selector.enabled = true;
            selector.material.color = Color.yellow;
        }
        else
        {
            selector.enabled = false;
        }
    }

    public void HighlightSimple(bool v)
    {
       
    }

    public void HighlightSelected(Card card, bool v)
    {
        if (v && IsAwaliable(card))
        {
            selector.material.color = Color.red;	
        }

        if (!v)
        {
            if (IsAwaliable(card))
            {
                selector.material.color = Color.yellow;
            }
            else
            {
                selector.enabled = false;
            }
        }
    }

    void OnMouseEnter()
    {
        Debug.Log("mouse enter");

        if (CardsPlayer.Instance.ActiveCard == null)
        {
            HighlightSimple(true);
        }
        else
        {
            CardEffect cardEffect = CardsPlayer.Instance.ActiveCard.CardAsset.CardEffects.FirstOrDefault(ce => ce.cardAim != CardEffect.CardAim.None);

            if (cardEffect != null)
            {
                if (cardEffect.cardAim == CardEffect.CardAim.Any)
                {
                    CardsPlayer.Instance.SelectAim(this);
                }

                if (cardEffect.cardAim == CardEffect.CardAim.Enemy && Player == null)
                {
                    CardsPlayer.Instance.SelectAim(this);
                }
            }


        }
    }

    void OnMouseExit()
    {
        HighlightSimple(false);
        bool shouldDehighlight = true;
        if (CardsPlayer.Instance.ActiveCard)
        {
            CardEffect cardEffect = CardsPlayer.Instance.ActiveCard.CardAsset.CardEffects.FirstOrDefault(ce => ce.cardAim != CardEffect.CardAim.None);
            if (cardEffect != null)
            {
                if (cardEffect.cardAim == CardEffect.CardAim.All || cardEffect.cardAim == CardEffect.CardAim.Enemies || cardEffect.cardAim == CardEffect.CardAim.You)
                {
                    shouldDehighlight = false;
                }
            }


        }
        if (shouldDehighlight)
        {
            CardsPlayer.Instance.SelectAim(null);
        }
    }
}
