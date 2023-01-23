using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PGCTerrain))]
[CanEditMultipleObjects]
public class PGCTerrainEditor : Editor
{
    SerializedProperty randomHeightRange;
 
    //sin stuff
    SerializedProperty period;
    SerializedProperty amplitude;
    SerializedProperty frequency;

    //perlin stuff
    SerializedProperty perlinX;
    SerializedProperty perlinY;
    SerializedProperty perlinHeight;

    bool showRandom = false;
    bool showCos = false;
    bool showPerlin = false;

    void Start()
    {
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }

    void OnEnable()
    {
        randomHeightRange = serializedObject.FindProperty("randomHeightRange");
        
        perlinX = serializedObject.FindProperty("perlinX");
        perlinY = serializedObject.FindProperty("perlinY");
        perlinHeight = serializedObject.FindProperty("perlinHeight");

        period = serializedObject.FindProperty("period");
        amplitude = serializedObject.FindProperty("amplitude");
        frequency = serializedObject.FindProperty("frequency"); //not sure if this is called something else

    }
  
    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        PGCTerrain terrain = (PGCTerrain)target;
        
        showRandom = EditorGUILayout.Foldout(showRandom, "Random");
        showCos = EditorGUILayout.Foldout(showCos, "Cosine");
        showPerlin = EditorGUILayout.Foldout(showPerlin, "Perlin");

        GUILayout.Label("Reset Terrain ", EditorStyles.boldLabel);
        if (GUILayout.Button("Reset"))
        {
            terrain.ResetTerrain();
        }

        if (showRandom)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Set Heights Between Random Values", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(randomHeightRange);

            if(GUILayout.Button("Random Heights"))
            {
                terrain.RandomTerrain();
            }
        }
        serializedObject.ApplyModifiedProperties();

        if (showPerlin)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Use perlin noise ", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(perlinX);
            EditorGUILayout.PropertyField(perlinY);
            EditorGUILayout.PropertyField(perlinHeight);


            if (GUILayout.Button("Perlin Noise"))
            {
                terrain.PerlinTerrain();
            }
        }
        serializedObject.ApplyModifiedProperties();
        if (showCos)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Use Cos curve", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(period);
            EditorGUILayout.PropertyField(amplitude);
            EditorGUILayout.PropertyField(frequency);

            if (GUILayout.Button("Cos Heights"))
            {
                terrain.CosTerrain();
            }
        }
        serializedObject.ApplyModifiedProperties();
         
    }
}
