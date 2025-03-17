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

        if (GUILayout.Button("Rebuild Road"))
        {
            generateRoad.Rebuild();
            SceneView.RepaintAll();
        }

    }
}

#endif