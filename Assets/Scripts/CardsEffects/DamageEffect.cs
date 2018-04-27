using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageEffect : ICardEffect
{
    public bool TryToPlayCard(List<CardEffect> effects, List<ISkillAim> aims, Action callback)
    {
        foreach (CardEffect observeEffect in effects)
        {
            if (observeEffect.warriorActionType != CardEffect.WariorActionType.Damage)
            {
                continue;
            }

            Debug.Log(aims.Count);
            if (aims.Count > 0)
            {
                foreach (ISkillAim p in aims)
                {
                    RPGCardGameManager.sInstance.Damage((WarriorObject)p, observeEffect.Value);
                }
                return true;
            }
        }
        return false;
    }
}
