using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEffect : ICardEffect
{

    public bool TryToPlayCard(List<CardEffect> effects, List<ISkillAim> aims, Action callback)
    {
        foreach (CardEffect observeEffect in effects)
        {
            if (observeEffect.warriorActionType != CardEffect.WariorActionType.Block)
            {
                continue;
            }
            if (aims.Count > 0)
            {
                foreach (ISkillAim p in aims)
                {
					RPGCardGameManager.sInstance.Block(((WarriorVisual)p).Warrior, observeEffect.Value);
                }
                return true;
            }
        }
        return false;
    }
}
