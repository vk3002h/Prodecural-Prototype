using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBuildingGenerator : MonoBehaviour
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

    [SerializeField]
    [Range(0f, 1f)]
    private float doorChance = 0.5f;

    [SerializeField]
    [Range(0f, 1f)]
    private float windowChance = 0.5f;

    [Range(1, 30)]
    public int rows = 3;

    [Range(1, 30)]
    public int columns = 3;

    [SerializeField]
    [Range(0f, 20f)]
    private float ceilSize = 1f;

    [SerializeField]
    private Material shaderMaterial;

    [Range(1, 30)]
    public int numberOfFloors = 3;

    public List<Vector3> RoomPositions = new List<Vector3>();

    private List<GameObject> rooms = new List<GameObject>();

    private int prefabCounter = 0;

    private Material material;

    [SerializeField]
    private float materialIndex;

    public void Generate()
    {
        MaterialColorRandomizer();
        GenerateDataPreset();
        Build();
    }
    void GenerateDataPreset()
    {
        for (int f = 0; f < numberOfFloors; f++)
        {
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    var roomPosition = new Vector3(transform.position.x + i * ceilSize, transform.position.y + f * ceilSize, transform.position.z + j * ceilSize);
                    RoomPositions.Add(roomPosition);
                }
            }
        }
    }

    void Build()
    {
        foreach (Vector3 vector3 in RoomPositions)
        {
            var i = Mathf.RoundToInt((vector3.x - transform.position.x) / ceilSize);
            var j = Mathf.RoundToInt((vector3.z - transform.position.z) / ceilSize);
            GameObject room = new GameObject($"Room_{i}_{j}");
            room.transform.position = vector3;
            rooms.Add(room);
            room.transform.parent = transform;
            if (transform.position.y >= vector3.y)
            {
                RoomPlacement(Random.Range(0.0f, 1.0f) <= doorChance ? doorPrefab : wallPrefab, room, i, j);
            }
            else
            {
                RoomPlacement(Random.Range(0.0f, 1.0f) <= windowChance ? windowPrefab[Random.Range(0, windowPrefab.Length)] : wallPrefab, room, i, j);
            }
        }
    }

    private void RoomPlacement(GameObject prefab, GameObject room, float row, float column)
    {
        PlacePrefab(floorPrefab, room.transform, room.transform.position, room.transform.rotation);
        if (includeRoof && Mathf.RoundToInt (room.transform.position.y) == Mathf.RoundToInt(transform.position.y + (numberOfFloors - 1) * ceilSize))
        {
            PlacePrefab(roofPrefab, room.transform, room.transform.position, room.transform.rotation);
        }
        if (row == rows - 1 || row == 0)
        {
            if (column == columns - 1)
            {
                PlacePrefab(prefab, room.transform, room.transform.position, room.transform.rotation);
                if(row == rows - 1)
                {
                    PlacePrefab(prefab, room.transform, room.transform.position, Quaternion.Euler(0, 90, 0));
                }
                else
                {
                    PlacePrefab(prefab, room.transform, room.transform.position, Quaternion.Euler(0, -90, 0));
                }
            }
            else if (column == 0)
            {
                PlacePrefab(prefab, room.transform, room.transform.position, Quaternion.Euler(0, 180, 0));
                if (row == rows - 1)
                {
                    PlacePrefab(prefab, room.transform, room.transform.position, Quaternion.Euler(0, 90, 0));
                }
                else
                {
                    PlacePrefab(prefab, room.transform, room.transform.position, Quaternion.Euler(0, -90, 0));
                }
            }
            else if (row == 0)
            {
                PlacePrefab(prefab, room.transform, room.transform.position, Quaternion.Euler(0, -90, 0));
            }
            else
            {
                PlacePrefab(prefab, room.transform, room.transform.position, Quaternion.Euler(0, 90, 0));
            }
        }
        else if (column == columns - 1)
        {
            PlacePrefab(prefab, room.transform, room.transform.position, room.transform.rotation);
        }
        else if(column == 0)
        {
            PlacePrefab(prefab, room.transform, room.transform.position, Quaternion.Euler(0, 180, 0));
        }
    }

    void PlacePrefab(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
    {
        GameObject gameObject = Instantiate(prefab, position, rotation);
        gameObject.GetComponentInChildren<Renderer>().material = material;
        gameObject.transform.parent = parent;
        gameObject.name = $"{gameObject.name}_{prefabCounter}";
        prefabCounter++;
    }

    void MaterialColorRandomizer()
    {
        materialIndex = Random.Range(0.0f, 1.0f);
        material = new Material(shaderMaterial);
        material.SetFloat("_Seed", materialIndex);
    }
}
