using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddModifierEffect : ICardEffect
{
    public bool TryToPlayCard(List<CardEffect> effects, List<ISkillAim> aims, Action callback)
    {
        foreach (CardEffect observeEffect in effects)
        {
            if (observeEffect.warriorActionType != CardEffect.WariorActionType.AddEffect)
            {
                continue;
            }
            if (aims.Count > 0)
            {
                foreach (ISkillAim p in aims)
                {
                    RPGCardGameManager.sInstance.AddModifier((WarriorObject)p, observeEffect.addingEffect, observeEffect.duration);
                }
                return true;
            }
        }
        return false;
    }
}
