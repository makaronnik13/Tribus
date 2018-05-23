using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tribus/Global/LevelSettings")]
public class LevelSettingsAsset : ScriptableObject
{
    public LevelSettings LevelSettings;
    public AnimationCurve BattlePRD, ChestPRD, ChallengePDR, EmptyRoomPRD, ShopPRD;

    public float GetPrd(int encounterId, int nonProcked)
    {
        switch (encounterId)
        {
            case 0:
                return EmptyRoomPRD.Evaluate(nonProcked) * LevelSettings.EmptyRoomChance/10;
            case 1:
                return BattlePRD.Evaluate(nonProcked) * LevelSettings.BattleChance / 10;
            case 2:
               return ChestPRD.Evaluate(nonProcked) * LevelSettings.ChestChance / 10;
            case 3:
                return ChallengePDR.Evaluate(nonProcked) * LevelSettings.ChallengeChance / 10;
            case 4:
                return ShopPRD.Evaluate(nonProcked) * LevelSettings.ShopChance / 10;
        }

        return 0;
    }

	public List<Item> Items;
    public List<Card> Cards;
    public float PercentGap;
	public float ShopPercentGap = 0.3f;
}
