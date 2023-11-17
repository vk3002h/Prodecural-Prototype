using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementHelper
{
    public static List<Direction> FindAdjacent(Vector3Int position, ICollection<Vector3Int> collection)
    {
        List<Direction> adjacentDirections = new List<Direction>();
        if (collection.Contains(position + Vector3Int.right))
        {
            adjacentDirections.Add(Direction.Right);
        }
        if (collection.Contains(position - Vector3Int.right))
        {
            adjacentDirections.Add(Direction.Left);
        }
        if (collection.Contains(position + new Vector3Int(0, 0, 1)))
        {
            adjacentDirections.Add(Direction.Up);
        }
        if (collection.Contains(position - new Vector3Int(0, 0, 1)))
        {
            adjacentDirections.Add(Direction.Down);
        }
        return adjacentDirections;
    }

    internal static Vector3Int GetOffset(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector3Int (0, 0, 1);
            case Direction.Down:
                return new Vector3Int(0, 0, -1);
            case Direction.Left:
                return Vector3Int.left;
            case Direction.Right:
                return Vector3Int.right;
            default:
                break;
        }
        throw new System.Exception("no such directions as: " + direction);
    }
}
