using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public int floorNumber { get; private set; }

    [SerializeField]
    public Room[,] rooms;

    public int Rows { get; private set; }

    public int Columns { get; private set;}

    public Floor(int floorNum, Room[,] rooms)
    {
        floorNumber = floorNum;
        this.rooms = rooms;
        Rows = rooms.GetLength(0);
        Columns = rooms.GetLength(1);
    }
}
