using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrainGeneratorWindow : EditorWindow
{

    [SerializeField]
    private int _nrIterations = 100;
    [SerializeField]
    TerrainGeneration _terrainGenerator = new TerrainGeneration();


    [MenuItem("Window/Terrain Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TerrainGeneratorWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        SerializedObject thisObj = new SerializedObject(this);

        SerializedProperty serNrIter = thisObj.FindProperty("_nrIterations");
        EditorGUILayout.PropertyField(serNrIter);

        SerializedProperty serHeighMap = thisObj.FindProperty("_terrainGenerator");
        EditorGUILayout.PropertyField(serHeighMap);
        GUILayout.Label("Simulate", EditorStyles.boldLabel);
        
        thisObj.ApplyModifiedProperties();
        if (GUILayout.Button("Simulate")) 
        {
            _terrainGenerator.Simulate(_nrIterations);
        }
        if (GUILayout.Button("Step")) 
        {
            _terrainGenerator.Simulate(1);
        }
    }
}
