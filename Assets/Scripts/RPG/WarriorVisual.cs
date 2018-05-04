using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WarriorVisual : MonoBehaviour, ISkillAim 
{
    private Action callback;
	private SpriteRenderer selector
	{
		get{
			return GetComponent<SpriteRenderer> ();
			}
	}

	public WarriorObject Warrior
	{
		get
		{
			return GetComponentInParent<WarriorObject> ();
		}
	}

    private Animator animator;
	private Animator Animator
	{
		get
		{
			if(!animator)
			{
				animator = GetComponent<Animator> ();
			}
			return animator;
		}
	}

	public enum SkillAnimation
	{
		Atack,
		Buff,
		Heal,
		Block,
		Super,
        Hit,
        Die
	}

	public void Animate(SkillAnimation animation, Action callback)
	{
        this.callback += callback;
		Animator.SetTrigger (animation.ToString());
	}

    public void TriggerCallback()
    {
        if (callback!=null)
        {
            callback();
            callback = null;
        }
    }

	public bool IsAwaliable(Card card)
	{
		if (card)
		{
			if(Warrior.hpSlider.Hp<=0)
			{
				return false;
			}
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

				if (cardEffect.cardAim == CardEffect.CardAim.Enemies && Warrior.Player == null)
				{
					return true;
				}

                if (cardEffect.cardAim == CardEffect.CardAim.Allies && Warrior.Player != null)
                {
                    return true;
                }

                if (cardEffect.cardAim == CardEffect.CardAim.Ally  && Warrior.Player != null)
                {
                    return true;
                }

                if (cardEffect.cardAim == CardEffect.CardAim.Enemy && Warrior.Player == null)
				{
					return true;
				}

				if (cardEffect.cardAim == CardEffect.CardAim.You && Warrior.Player == RPGCardGameManager.sInstance.CurrentPlayer.photonPlayer)
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
			selector.material.SetFloat ("_OutlineSize", 1);
			selector.material.SetColor ("_OutlineColor", new Color(1,1,0,0.75f));
		}
		else
		{
			selector.material.SetFloat ("_OutlineSize", 0);
		}
	}

	public void HighlightSimple(bool v)
	{

	}

	public void HighlightSelected(Card card, bool v)
	{
		if (v && IsAwaliable(card))
		{
			selector.material.SetColor ("_OutlineColor", new Color(1,0,0,0.75f));
		}

		if (!v)
		{
			if (IsAwaliable(card))
			{
				selector.material.SetColor ("_OutlineColor", new Color(1,1,0,0.75f));
			}
			else
			{
				selector.material.SetFloat ("_OutlineSize", 0);
			}
		}
	}

	void OnMouseEnter()
	{
		if (CardsPlayer.Instance.ActiveCard == null)
		{
			HighlightSimple(true);
		}
		else
		{
			CardEffect cardEffect = CardsPlayer.Instance.ActiveCard.CardAsset.CardEffects.FirstOrDefault(ce => ce.cardAim != CardEffect.CardAim.None);

			if (cardEffect != null)
			{
				if (IsAwaliable(CardsPlayer.Instance.ActiveCard.CardAsset))
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
