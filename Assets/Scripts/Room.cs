using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Wall[] Walls { get; set; } = new Wall[4];

    private Vector2 position;

    public bool hasRoof { get; private set; }

    public int floorNumber { get; set; }

    public Room(Vector2 position, bool hasRoof = false)
    {
        this.position = position;
        this.hasRoof = hasRoof; 
    }

    public Vector2 roomPosition
    {
        get
        {
            return this.position;
        }
    }
}
