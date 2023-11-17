using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralCity/Rule")]
public class Rules : ScriptableObject
{
    public string letter;
    [SerializeField]
    private string[] results = null;
    [SerializeField]
    private bool randomResult = false;

    public string GetResult()
    {
        if (randomResult)
        {
            int randomLength = Random.Range(0, results.Length);
            return results[randomLength];
        }
        return results[0];
    }
}

