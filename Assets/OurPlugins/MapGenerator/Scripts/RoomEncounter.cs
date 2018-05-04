using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEncounter
{
    public enum RoomEncounterState
    {
        Battle,
        Chalenge
    }

    public RoomEncounterState EncounterType;
    public Chalenge Chelenge;
    public Battle Battle;
}
