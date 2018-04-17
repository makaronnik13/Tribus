using System.Collections.Generic;
using System;

public interface ICardEffect
{
	bool TryToPlayCard(List<CardEffect> effect, List<ISkillAim> aims, Action callback);
}
