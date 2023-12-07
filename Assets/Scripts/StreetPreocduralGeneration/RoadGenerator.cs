using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject road, roadCorner, roadThreeCrossing, roadFourCrossing, roadEnd;
    Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
    HashSet<Vector3Int> roadFixer = new HashSet<Vector3Int>();


    public List<Vector3Int> GetRoadPsoition()
    {
        return roadDictionary.Keys.ToList();
    }
    public void RoadPlacement(Vector3 start, Vector3Int direction, int length)
    {
        var rotation = Quaternion.identity;
        if (direction.x == 0)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }
        for (int i = 0; i < length; i++)
        {
            var position = Vector3Int.RoundToInt(start + direction * i);
            if (roadDictionary.ContainsKey(position))
            {
                continue;
            }
            var roadtile = Instantiate(road, position, rotation, transform);
            roadDictionary.Add(position, roadtile);
            if (i == length - 1 || i == 0)
            {
                roadFixer.Add(position);
            }
        }
    }

    public void RoadFix()
    {
        foreach(var positions in roadFixer)
        {
            List<Direction> adjacentDirection = PlacementHelper.FindAdjacent(positions, roadDictionary.Keys);

            Quaternion rotation = Quaternion.identity;

            if (adjacentDirection.Count == 1)
            {
                Destroy(roadDictionary[positions]);
                if (adjacentDirection.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0,90,0);
                }
                else if (adjacentDirection.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (adjacentDirection.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDictionary[positions] = Instantiate(roadEnd, positions, rotation, transform);
            }
            else if (adjacentDirection.Count == 2)
            {
                if(adjacentDirection.Contains(Direction.Up) && adjacentDirection.Contains(Direction.Down)
                    || adjacentDirection.Contains(Direction.Left) && adjacentDirection.Contains(Direction.Right))
                {
                    continue;
                }
                Destroy(roadDictionary[positions]);
                if (adjacentDirection.Contains(Direction.Up) && adjacentDirection.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (adjacentDirection.Contains(Direction.Right) && adjacentDirection.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (adjacentDirection.Contains(Direction.Down) && adjacentDirection.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDictionary[positions] = Instantiate(roadCorner, positions, rotation, transform);
            }
            else if (adjacentDirection.Count == 3)
            {
                Destroy(roadDictionary[positions]);
                if (adjacentDirection.Contains(Direction.Down) && 
                    adjacentDirection.Contains(Direction.Right) &&
                    adjacentDirection.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (adjacentDirection.Contains(Direction.Down) && 
                    adjacentDirection.Contains(Direction.Left) &&
                    adjacentDirection.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (adjacentDirection.Contains(Direction.Up) && 
                    adjacentDirection.Contains(Direction.Left) &&
                    adjacentDirection.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDictionary[positions] = Instantiate(roadThreeCrossing, positions, rotation, transform);
            }
            else
            {
                Destroy(roadDictionary[positions]);
                roadDictionary[positions] = Instantiate(roadFourCrossing, positions, rotation, transform);
            }


        }
    }

    public void Reset()
    {
        foreach(var item in roadDictionary.Values)
        {
            Destroy(item);
        }
        roadDictionary.Clear();
        roadFixer = new HashSet<Vector3Int>();
    }
}
