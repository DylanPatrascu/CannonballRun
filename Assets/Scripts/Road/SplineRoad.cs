//using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
//using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
//using UnityEngine.Splines;

[ExecuteInEditMode]
public class SplineRoad : MonoBehaviour
{

    [SerializeField] SplineSampler m_splineSampler;
    [Range(1, 10000)][SerializeField] private int resolution;
    [SerializeField] private float m_roadWidth = 20;
    [SerializeField] private MeshFilter m_meshFilter;
    [SerializeField] private MeshCollider m_meshCollider;

    List<float3> m_vertsP1;
    List<float3> m_vertsP2;

    void Start()
    {
        Rebuild();
    }

    private void OnEnable()
    {
        m_splineSampler.SplineSamplerValidate += OnValidate;
    }
    private void OnDisable()
    {
        m_splineSampler.SplineSamplerValidate -= OnValidate;
    }

    private void OnValidate()
    {
        if (gameObject.activeInHierarchy) StartCoroutine(ValidateRebuild());
    }

    public IEnumerator ValidateRebuild()
    {
        yield return null;
        Rebuild();
    }

    private void GetVerts()
    {
        m_vertsP1 = new List<float3>();
        m_vertsP2 = new List<float3>();

        float step = 1f / (float) resolution;
        for (int i = 0; i < resolution; i++)
        {
            float t = step * i;
            m_splineSampler.SampleSplineWidth(t, m_roadWidth, out Vector3 p1, out Vector3 p2);
            m_vertsP1.Add(p1);
            m_vertsP2.Add(p2);
        }
        
    }

    private void BuildMesh()
    {
        Mesh m = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        int offset = 0;

        int length = m_vertsP2.Count;

        for (int i = 1; i < length; i++)
        {

            Vector3 p1 = m_vertsP1[i-1];
            Vector3 p2 = m_vertsP2[i-1];
            Vector3 p3 = m_vertsP1[i];
            Vector3 p4 = m_vertsP2[i];

            offset = 4 * (i - 1);

            verts.AddRange(new List<Vector3> { p1, p2, p3, p4 });
            tris.AddRange(new List<int> 
            { 
                offset, offset + 2,  offset + 3, 
                offset + 3, offset + 1, offset 
            });

        }

        m.Clear();
        m.SetVertices(verts);
        m.SetTriangles(tris, 0);
        m.RecalculateNormals();

        m.name = "Road Mesh";
        m_meshFilter.mesh = m;

        m_meshCollider.sharedMesh = null;
        m_meshCollider.sharedMesh = m;
    }

    public void Rebuild()
    {
        EditorApplication.delayCall -= Rebuild;
        GetVerts();
        BuildMesh();
    }

}
