#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Buildings))]
public class BuildingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Buildings buildings = (Buildings)target;

        if (Application.isPlaying)
        {
            if (GUILayout.Button("Generate Buildings"))
            {
                if (buildings.m_coroutine != null) buildings.StopCoroutine(buildings.m_coroutine);
                buildings.m_coroutine = buildings.StartCoroutine(buildings.SwapBuildings());
            }
        }
    }
}

#endif