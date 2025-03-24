using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class GenerateRoad : MonoBehaviour
{

    [SerializeField] private SplineContainer m_splineContainer;

    [SerializeField] private Transform m_startPoint;
    [SerializeField] private Transform m_endPoint;
    
    [SerializeField] private int m_knots;

    [SerializeField] private bool m_randomize;

    private float m_distance;
    private float m_knotDistance;
    private Spline m_road;

    private void OnValidate()
    {
        if (m_randomize) Rebuild();
    }

    public void Rebuild()
    {
        m_distance = Vector3.Distance(m_startPoint.position, m_endPoint.position);
        m_knotDistance = m_distance / m_knots;
        RebuildSpline();
        SplineRoad sr = GetComponent<SplineRoad>();
        if (sr != null && gameObject.activeInHierarchy) { sr.StartCoroutine(sr.ValidateRebuild()); }
    }

    public void RebuildSpline()
    {
        
        if (m_splineContainer.Spline != null)
        {
            m_splineContainer.Spline.Clear();
        }
                

        m_road = new Spline();

        float slope = (m_startPoint.position.y - m_endPoint.position.y) / (m_startPoint.position.x - m_endPoint.position.x);

        m_road.Add(new BezierKnot(m_startPoint.position + new Vector3(0, 0, -500)));
        m_road.Add(new BezierKnot(m_startPoint.position, Vector3.zero, new Vector3(0, 0, m_knotDistance)));
        for (int i = 0; i < m_knots; i++)
        {
            float t = (i + 1) / (float)(m_knots + 1);
            BezierKnot knot = new BezierKnot();

            if (m_road.Knots.Count() > 1)
                while (Vector3.Distance((Vector3)knot.Position, (Vector3)m_road.Knots.Last().Position) < m_knotDistance
                    || Vector3.Distance((Vector3)knot.Position, (Vector3)m_road.Knots.ElementAt(m_road.Knots.Count() - 2).Position) < m_knotDistance
                    || Vector3.Distance((Vector3)knot.Position, m_endPoint.position) < m_knotDistance
                    || knot.Position.z - m_road.Knots.Last().Position.z <= 0)
                {
                    knot.Position = Vector3.Lerp(m_startPoint.position, m_endPoint.position, t);
                    knot.Position += new float3(UnityEngine.Random.Range(-m_knotDistance, m_knotDistance), 0, UnityEngine.Random.Range(-m_knotDistance, m_knotDistance));
                
                }
            else
                while (Vector3.Distance((Vector3)knot.Position, (Vector3)m_road.Knots.Last().Position) < m_knotDistance
                    || knot.Position.z - m_road.Knots.Last().Position.z <= 0)
                {
                    knot.Position = Vector3.Lerp(m_startPoint.position, m_endPoint.position, t);
                    knot.Position += new float3(UnityEngine.Random.Range(-m_knotDistance, m_knotDistance), 0, UnityEngine.Random.Range(-m_knotDistance, m_knotDistance));

                }
            knot.TangentIn = new float3 (0, 0, -m_knotDistance / 2);
            knot.TangentOut = new float3 (0, 0, m_knotDistance / 2);
            m_road.Add(knot);
        }
        m_road.Add(new BezierKnot(m_endPoint.position, new Vector3(0, 0, -m_knotDistance), Vector3.zero));
        m_road.Add(new BezierKnot(m_endPoint.position + new Vector3(0, 0, 500)));

        m_splineContainer.Spline = m_road;
    }

    public bool randomizeEnabled()
    {
        return m_randomize;
    }

}
