using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    [SerializeField]
    private GameObject wallPrefab;

    [SerializeField]
    private GameObject roofPrefab;

    [SerializeField]
    private GameObject floorPrefab;

    [SerializeField]
    private GameObject[] windowPrefab;

    [SerializeField]
    private GameObject doorPrefab;

    [SerializeField]
    private bool includeRoof = false;

    [Header("Generation Options")]
    [SerializeField]
    [Range(0f, 1f)]
    private float doorChance = 0.5f;

    [SerializeField]
    [Range(0f, 1f)]
    private float windowChance = 0.5f;

    [Range(1, 70)]
    public int rows = 3;

    [Range(1, 70)]
    public int columns = 3;

    [SerializeField]
    [Range(0f, 20f)]
    private float ceilSize = 1f;

    [Range(1, 30)]
    public int numberOfFloors = 3;

    private Floor[] floors;

    //private void Awake() => Generate();//

    private List<GameObject> overlapList = new List<GameObject>();

    private List <GameObject> rooms = new List <GameObject>();

    private int prefabCounter = 0;

    public void Generate()
    {
        prefabCounter = 0;

        Clear();

        BuildData();

        Render();

        CleanInsidewalls();
    }

    void BuildData()
    {
        floors = new Floor[numberOfFloors];

        int floorCount = 0;

        foreach (Floor floor in floors)
        {
            Room[,] rooms = new Room[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var roomPosition = new Vector3(transform.position.x + i * ceilSize, transform.position.y + floorCount * ceilSize, transform.position.z + j * ceilSize);
                    rooms[i, j] = new Room(new Vector2(i*ceilSize, j*ceilSize), includeRoof ? (floorCount == floors.Length - 1) : false);

                    rooms[i, j].Walls[0] = new Wall(roomPosition, Quaternion.Euler(0, 0, 0));
                    rooms[i, j].Walls[1] = new Wall(roomPosition, Quaternion.Euler(0, 90, 0));
                    rooms[i, j].Walls[2] = new Wall(roomPosition, Quaternion.Euler(0, 180, 0));
                    rooms[i, j].Walls[3] = new Wall(roomPosition, Quaternion.Euler(0, -90, 0));
                }
            }
            floors[floorCount] = new Floor(floorCount++, rooms);
        }
    }

    void Render()
    {
        foreach (Floor floor in floors)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Room room = floor.rooms[i, j];
                    GameObject roomPa = new GameObject($"Room_{i}_{j}");
                    rooms.Add(roomPa);
                    roomPa.transform.parent = transform;
                    if (floor.floorNumber == 0)
                    {
                        RoomPlacement(Random.Range(0.0f, 1.0f) <= doorChance ? doorPrefab : wallPrefab, room, roomPa);
                    }
                    else
                    {
                        RoomPlacement(Random.Range(0.0f, 1.0f) <= windowChance ? windowPrefab[Random.Range(0,windowPrefab.Length)] : wallPrefab, room, roomPa);
                    }
                }
            }
        }
    }

    private void RoomPlacement(GameObject prefab, Room room, GameObject roomPa)
    {
        SpawnPrefab(prefab, roomPa.transform, room.Walls[0].Position, room.Walls[0].Rotation);
        SpawnPrefab(prefab, roomPa.transform, room.Walls[1].Position, room.Walls[1].Rotation);
        SpawnPrefab(prefab, roomPa.transform, room.Walls[2].Position, room.Walls[2].Rotation);
        SpawnPrefab(prefab, roomPa.transform, room.Walls[3].Position, room.Walls[3].Rotation);
        SpawnPrefab(floorPrefab, roomPa.transform, room.Walls[0].Position, room.Walls[0].Rotation);
        if (room.hasRoof)
        {
            SpawnPrefab(roofPrefab, roomPa.transform, room.Walls[0].Position, room.Walls[0].Rotation);
        }
    }

    private void SpawnPrefab(GameObject prefab, Transform parent, Vector3 position, Quaternion rotastion)
    {
        GameObject gameObject = Instantiate(prefab, position, rotastion);
        gameObject.transform.parent = parent;
        gameObject.name = $"{gameObject.name}_{prefabCounter}";
        prefabCounter++;
    }

    void Clear()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            DestroyImmediate(rooms[i]);
        }
        rooms.Clear();
    }

    void CleanInsidewalls()
    {

    }
}
