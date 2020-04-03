using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tribus
{
public class MapGenerator : MonoBehaviour
{
    public float hardIncreasing = 1;
    public GameObject roomPrefab;
	public List<Room> Generate()
    {

        foreach (Transform t in transform)
        {
            //    Destroy(t.gameObject);
        }

        List<Room> rooms = new List<Room>();

        foreach (Vector2 v in GeneratePositions(FindObjectOfType<RoomMap>().LevelSettings.CellsNumber))
        {
            GameObject newRoom = Instantiate(roomPrefab, transform);
            newRoom.transform.localScale = Vector3.one;
            newRoom.transform.localPosition = new Vector3(v.x, v.y, 0);
            Room room = newRoom.GetComponent<Room>();
            room.Init(v);
            rooms.Add(room);
        }

        return rooms;
    }

    private List<Vector2> GeneratePositions(int rooms)
    {
        List<Vector2> positions = new List<Vector2>();
        positions.Add(Vector2.zero);

        while (positions.Count<rooms)
        {
            positions.Add(GetNewRoom(positions));
        }

        return positions;
    }

    private Vector2 GetNewRoom(List<Vector2> positions)
    {
        List<Vector2> avaliableSpaces = new List<Vector2>();

        foreach (Vector2 p in positions)
        {
            avaliableSpaces.Add(p+Vector2.up);
            avaliableSpaces.Add(p - Vector2.up);
            avaliableSpaces.Add(p + Vector2.left);
            avaliableSpaces.Add(p - Vector2.left);
        }

        avaliableSpaces = avaliableSpaces.Distinct().ToList();
        avaliableSpaces.RemoveAll(p=>positions.Contains(p));

        Dictionary<float, Vector2> cellChances = new Dictionary<float, Vector2>();

        float globalChance = 0;
        foreach (Vector2 p in avaliableSpaces)
        {
            float cellChance = GetCellChance(p, positions);
            globalChance += cellChance;
            cellChances.Add(globalChance, p);
        }

        float chance = UnityEngine.Random.Range(0, globalChance);

        float probability = UnityEngine.Random.Range(0, globalChance);
        Vector2 selected = cellChances.SkipWhile(i => i.Key < probability).First().Value;

        return selected;
    }

    private float GetCellChance(Vector2 cell, List<Vector2> filledCells)
    {
        int nearCells = GetNearCells(cell, filledCells).Count();

        if (nearCells == 1)
        {
            return 25;
        }

        return 1;
    }

    private List<Vector2> GetNearCells(Vector2 cell, List<Vector2> filledCells)
    {
        List<Vector2> checkingPositions = new List<Vector2>();
        checkingPositions.Add(cell + Vector2.up);
        checkingPositions.Add(cell - Vector2.up);
        checkingPositions.Add(cell + Vector2.left);
        checkingPositions.Add(cell - Vector2.left);
        return filledCells.Intersect(checkingPositions).ToList();
    }
}
}
