using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsPlayer : Singleton<CardsPlayer>
{
	public List<GameObject> focusedAims = new List<GameObject> ();

    public Action<Card> OnCardPlayed = (Card card) => { };

    public void PlayCard(CardVisual card)
    {
		PlayCard(card, focusedAims);
    }

	public void PlayCard(CardVisual card, List<GameObject> aims)
    {
		Debug.Log (card.CardAsset+" "+aims.Count);
		CardsManager.Instance.OnCardDroped.Invoke (card);
		foreach(GameObject aim in aims)
		{
			if(aim.GetComponent<Block>())
			{
				SkillsController.Instance.ActivateSkill (aim.GetComponent<Block>(), card.CardAsset.skill, card.CardAsset.skillLevel);
			}
		}
		OnCardPlayed.Invoke(card.CardAsset);
    }

	public void SelectBlock(Block block)
	{
		focusedAims = new List<GameObject> ();
		focusedAims.Add (block.gameObject);
	}
}
