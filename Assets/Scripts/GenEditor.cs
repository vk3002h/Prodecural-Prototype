using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenEditor : Editor
{
    [CustomEditor(typeof(Generation))]
    public override void OnInspectorGUI()
    {
        Debug.Log("Editor Script");

        DrawDefaultInspector();

        Generation genEditor = (Generation)target;

        if (GUILayout.Button("Generate"))
        {
            genEditor.Generate();
        }

        if (GUI.changed)
        {
            genEditor.Generate();
        }
    }
}
