#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenerateRoad))]
public class GenerateRoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GenerateRoad generateRoad = (GenerateRoad)target;

        GUI.enabled = (generateRoad.randomizeEnabled());
        if (GUILayout.Button("Rebuild Road"))
        {
            generateRoad.Rebuild();
            SceneView.RepaintAll();
        }
        GUI.enabled = true;

    }
}

#endif