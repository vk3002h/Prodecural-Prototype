using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPrefab : MonoBehaviour
{
    public int buildingMaxWidth = 6;
    public int buildingMaxHeight = 6;
    public int buildingMaxLength = 6;
    public GameObject child;

    private bool generated = false;
    private int width;
    private int height;
    private int length;
    // Start is called before the first frame update
    void Start()
    {
        width = Random.Range(2, buildingMaxWidth);
        height = Random.Range(2,buildingMaxHeight);
        length = Random.Range(2,buildingMaxLength);
    }

    // Update is called once per frame
    void Update()
    {
        if (child.GetComponent<Generation>())
        {
            child.GetComponent<Generation>().rows = length;
            child.GetComponent<Generation>().columns = width;
            child.GetComponent<Generation>().numberOfFloors = height;
            if (!generated)
            {
                child.GetComponent<Generation>().Generate();
            }
            float tempX = (length / 2f) * 0.2f - 0.1f;
            float tempZ = (width / 2f) * 0.2f - 0.1f;
            child.transform.localPosition = new Vector3 (-tempX, child.transform.localPosition.y, -tempZ);
        }
    }
}
