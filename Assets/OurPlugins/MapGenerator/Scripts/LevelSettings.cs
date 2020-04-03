using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSettings
{
    public string LevelName;
    public Sprite LevelSprite;

    public Battle[] Battles;
    public Chalenge[] Chalenges;
    public int EmptyRooms;
    public int Shops;
    public int Chests;

    public int CellsNumber
    {
        get
        {
            return Chalenges.Length + Battles.Length + EmptyRooms + Shops + Chests;
        }
    }

    public float ChestChance
    {
        get
        {
            return (Chests+0.0f)/CellsNumber;
        }
    }

    public float EmptyRoomChance
    {
        get
        {
            return (EmptyRooms + 0.0f) / CellsNumber;
        }
    }

    public float ChallengeChance
    {
        get
        {
            return (Chalenges.Length + 0.0f) / CellsNumber;
        }
    }

    public float ShopChance
    {
        get
        {
            return (Shops + 0.0f) / CellsNumber;
        }
    }

    public float BattleChance
    {
        get
        {
            return (Battles.Length + 0.0f) / CellsNumber;
        }
    }
}
