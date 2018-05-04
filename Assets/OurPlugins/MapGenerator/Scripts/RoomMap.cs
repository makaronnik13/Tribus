using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tribus
{
public class RoomMap : MonoBehaviour {

    private List<Room> rooms = new List<Room>();

    private List<Vector2> emptySpaces = new List<Vector2>();

    public LevelSettingsAsset LevelSettingsAsset;
    public LevelSettings LevelSettings
    {
        get
        {
            return LevelSettingsAsset.LevelSettings;
        }
    }

    public Transform spaces;
    public GameObject emptySpacePrefab;
    public GameObject PartyPrefab;
    private GameObject party;
    public Vector2 CurrentPartyPosition = Vector2.zero;
    private bool partyMoving = false;
    public bool PartyMoving
    {
        get
        {
            return partyMoving;
        }
        set
        {
            partyMoving = value;
            if (!value)
            {
                UpdateCellsAvaliablility();
            }
        }
    }

    private void UpdateCellsAvaliablility()
    {
        foreach (Room r in rooms)
        {
            if (r.CurrentRoomState == Room.RoomState.Near && !IsRoomNear(r, CurrentPartyPosition))
            {
                r.CurrentRoomState = Room.RoomState.Far;
            }

            if (IsRoomNear(r, CurrentPartyPosition))
            {
                r.CurrentRoomState = Room.RoomState.Near;
            }
        }

        ShowEmptySpaces(CurrentPartyPosition);
    }

    private void ShowEmptySpaces(Vector2 pos)
    {
        List<Vector2> spaces = new List<Vector2>();

        spaces.Add(pos + Vector2.up);
        spaces.Add(pos - Vector2.up);
        spaces.Add(pos + Vector2.left);
        spaces.Add(pos - Vector2.left);

        foreach (Room r in NearestRoom(pos))
        {
            spaces.Remove(r.position);
        }

        foreach (Vector2 v in spaces)
        {
            if (!emptySpaces.Contains(v))
            {
                ShowEmptySpace(v);
            }
        }
    }

    private List<Room> NearestRoom(Vector2 pos)
    {
        List<Room> nearestrooms = new List<Room>();
        foreach (Room r in rooms)
        {
            if (IsRoomNear(r, pos))
            {
                nearestrooms.Add(r);
            }
        }
        return nearestrooms;
    }

    private void ShowEmptySpace(Vector2 pos)
    {
        GameObject es = Instantiate(emptySpacePrefab, spaces);
        es.transform.localPosition = pos;
        emptySpaces.Add(pos);
    }

    private bool IsRoomNear(Room r, Vector2 pos)
    {
        List<Vector2> avaliableSpaces = new List<Vector2>();
        avaliableSpaces.Add(pos + Vector2.up);
        avaliableSpaces.Add(pos - Vector2.up);
        avaliableSpaces.Add(pos + Vector2.left);
        avaliableSpaces.Add(pos - Vector2.left);
        avaliableSpaces.Add(pos);
        return avaliableSpaces.Contains(r.position);
    }

    // Use this for initialization
    void Start ()
    {
        rooms = GetComponent<MapGenerator>().Generate();
        GetRoomByCoord(Vector2.zero).visited = true;
        GetRoomByCoord(Vector2.zero).CurrentRoomState = Room.RoomState.Near;
        party = Instantiate(PartyPrefab);
        party.transform.position = transform.position;
        CurrentPartyPosition = party.transform.position;
        UpdateCellsAvaliablility();
	}

    private  Room GetRoomByCoord(Vector2 pos)
    {
        return rooms.FirstOrDefault(r=>r.position == pos);  
    }

    public void TryToGoTo(Room room)
    {
        if (PartyMoving || GetRoomByCoord(CurrentPartyPosition)==room)
        {
            return;
        }
        CurrentPartyPosition = room.position;
        StartCoroutine(MoveParty(CurrentPartyPosition));
    }

    private IEnumerator MoveParty(Vector2 currentPartyPosition)
    {
        PartyMoving = true;
        float t = 0;
        while (t<1)
        {
            party.transform.position = Vector3.Lerp(party.transform.position, currentPartyPosition, t);
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        GetRoomByCoord(CurrentPartyPosition).ComeToRoom();
        PartyMoving = false;
    }

}
}
