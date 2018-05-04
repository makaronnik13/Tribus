using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tribus
{
public class RoomActivator : MonoBehaviour
{
    public Action OnEmptyRoomIn = ()=> { };
    public Action<Battle> OnBattleIn = (Battle battle) => { };
    public Action OnShopIn = () => { };
    public Action<Chalenge> OnChellengeIn = (Chalenge ch) => { };
    public Action OnChestIn = () => { };

    private int visitedChests = 0;
    private int visitedEmptyRooms= 1;
    private int visitedShops = 0;
    private List<Battle> stayedBattles;
    private List<Chalenge> stayedChallenges;

    private int ChestedNonProckedTurns = 0;
    private int EmptyRoomsNonProckedTurns = 0;
    private int ShopsNonProckedTurns = 0;
    private int ChallengesNonProckedTurns = 0;
    private int BattlesNonProckedTurns = 0;

    private LevelSettings levelSettings
    {
        get
        {
            return FindObjectOfType<RoomMap>().LevelSettings;
        }
    }

	public void ActivateEncouter(bool roomVisited)
    {
        //fill battles and challenges if they are empty
        if (stayedBattles == null)
        {
            stayedBattles = FindObjectOfType<RoomMap>().LevelSettings.Battles.ToList();
            stayedChallenges = FindObjectOfType<RoomMap>().LevelSettings.Chalenges.ToList();
        }

        if (roomVisited)
        {
            ActivateEmptyRoomPanel();
            return;
        }

        int encounterType = 0;
        object nextEncounter = GetRandomEncounter(FindObjectOfType<PlayerStats>().CurrentLevel, out encounterType);

        switch (encounterType)
        {
            case 0:
                ActivateEmptyRoomPanel();
                break;
            case 1:
                StartBattle((Battle)nextEncounter);
                break;
            case 2:
                ShowChest(FindObjectOfType<PlayerStats>().CurrentLevel);
                break;
            case 3:
                StartChellenge((Chalenge)nextEncounter);
                break;
            case 4:
                ShowShop(FindObjectOfType<PlayerStats>().CurrentLevel);
                break;
        }

        FindObjectOfType<PlayerStats>().CurrentLevel++;
    }

    private void ShowShop(int currentLevel)
    {
        ChestedNonProckedTurns++;
        EmptyRoomsNonProckedTurns++;
        ChallengesNonProckedTurns++;
        BattlesNonProckedTurns++;
        visitedShops++;
        OnShopIn();
    }

    private void StartChellenge(Chalenge chelenge)
    {
        ChestedNonProckedTurns++;
        EmptyRoomsNonProckedTurns++;
        ShopsNonProckedTurns++;
        BattlesNonProckedTurns++;
        stayedChallenges.Remove(chelenge);
        OnChellengeIn(chelenge);
    }

    private void ShowChest(float lvl)
    {
        ShopsNonProckedTurns++;
        EmptyRoomsNonProckedTurns++;
        ChallengesNonProckedTurns++;
        BattlesNonProckedTurns++;
        visitedChests++;
        OnChestIn();
    }

    private void StartBattle(Battle battle)
    {
        ChestedNonProckedTurns++;
        EmptyRoomsNonProckedTurns++;
        ChallengesNonProckedTurns++;
        ShopsNonProckedTurns++;
        stayedBattles.Remove(battle);
        OnBattleIn(battle);
    }

    private void ActivateEmptyRoomPanel()
    {
        ChestedNonProckedTurns++;
        ShopsNonProckedTurns++;
        ChallengesNonProckedTurns++;
        BattlesNonProckedTurns++;
        visitedEmptyRooms++;
        OnEmptyRoomIn();
    }

    private object GetRandomEncounter(int currentLevel, out int encounterType)
    {
        Dictionary<int, float> encoutesrsChances = GetEncountersChances(
            ChestedNonProckedTurns, 
            EmptyRoomsNonProckedTurns,
            ShopsNonProckedTurns,
            ChallengesNonProckedTurns,
            BattlesNonProckedTurns);

        float globalChance = encoutesrsChances.ElementAt(encoutesrsChances.Count-1).Value;
 

        //Debug.Log(globalChance);
        foreach (KeyValuePair<int, float> pair in encoutesrsChances)
        {
            //Debug.Log(pair.Key+" - "+pair.Value);
        }
        

        float probability = UnityEngine.Random.Range(0, globalChance);
        encounterType = encoutesrsChances.SkipWhile(i => i.Value < probability).First().Key;

        switch (encounterType)
        {
            case 0:
                if (visitedEmptyRooms== FindObjectOfType<RoomMap>().LevelSettings.EmptyRooms)
                {
                    return GetRandomEncounter(currentLevel, out encounterType);
                }
                break;
            case 1:
                if (stayedBattles.Count == 0)
                {
                    return GetRandomEncounter(currentLevel, out encounterType);
                }
                return GetBestBattle(currentLevel);
            case 2:
                if (visitedChests== FindObjectOfType<RoomMap>().LevelSettings.Chests)
                {
                    return GetRandomEncounter(currentLevel, out encounterType);
                }
                break;
            case 3:
                if (stayedChallenges.Count == 0)
                {
                    return GetRandomEncounter(currentLevel, out encounterType);
                }
                return GetBestChallenge(currentLevel);
            case 4:
                if (visitedShops == FindObjectOfType<RoomMap>().LevelSettings.Shops)
                {
                    return GetRandomEncounter(currentLevel, out encounterType);
                }
                break;
        }

        return null;
    }

    private Battle GetBestBattle(int currentLevel)
    {
        Battle result = stayedBattles.OrderBy(n=> Mathf.Abs(currentLevel-n.HardLvl)).ToList()[0];
        return result;
    }

    private Chalenge GetBestChallenge(int currentLevel)
    {
        Chalenge result = stayedChallenges.OrderBy(n => Mathf.Abs(currentLevel - n.HardLvl)).ToList()[0];
        return result;
    }

    private Dictionary<int, float> GetEncountersChances(int chestedNonProckedTurns, int emptyRoomsNonProckedTurns, int shopsNonProckedTurns, int challengesNonProckedTurns, int battlesNonProckedTurns)
    {
        float emptyCahnce = FindObjectOfType<RoomMap>().LevelSettingsAsset.GetPrd(0, emptyRoomsNonProckedTurns);
        float battlechance = emptyCahnce + FindObjectOfType<RoomMap>().LevelSettingsAsset.GetPrd(1, battlesNonProckedTurns);
        float chestChance = battlechance + FindObjectOfType<RoomMap>().LevelSettingsAsset.GetPrd(2, chestedNonProckedTurns);
        float chellengeChance = chestChance + FindObjectOfType<RoomMap>().LevelSettingsAsset.GetPrd(3, challengesNonProckedTurns);
        float shopChance = chellengeChance + FindObjectOfType<RoomMap>().LevelSettingsAsset.GetPrd(4, shopsNonProckedTurns);
        return new Dictionary<int, float>()
        {
            {0, emptyCahnce},
            {1, battlechance},
            {2, chestChance},
            {3, chellengeChance},
            {4, shopChance}
        };
    }
}
}
