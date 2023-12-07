using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsGenerator : MonoBehaviour
{
    public GameObject buildingPrefab;
    public Dictionary<Vector3Int, GameObject> buildingDictionary = new Dictionary<Vector3Int, GameObject>();

    public void PlaceBuildings(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int,Direction> placableLocations = FreeSpaceFinder(roadPositions);
        foreach (var positions in placableLocations.Keys)
        {
            var building = Instantiate(buildingPrefab,positions, Quaternion.identity, transform);
            buildingDictionary.Add(positions, building);
        }
    }

    private Dictionary<Vector3Int,Direction> FreeSpaceFinder(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
        foreach (var position in roadPositions)
        {
            var adjacentDirections = PlacementHelper.FindAdjacent(position, roadPositions);
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (!adjacentDirections.Contains(direction))
                {
                    var newPosition = position + PlacementHelper.GetOffset(direction);
                    if (freeSpaces.ContainsKey(newPosition))
                    {
                        continue;
                    }
                    freeSpaces.Add(newPosition, Direction.Right);
                }

            }
        }
        return freeSpaces;
    }

    public void Reset()
    {
        foreach (var item in buildingDictionary.Values)
        {
            Destroy(item);
        }
        buildingDictionary.Clear();
    }
}
