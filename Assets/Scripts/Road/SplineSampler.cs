using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class SplineSampler : MonoBehaviour
{

    [SerializeField] private SplineContainer m_splineContainer;

    private int m_splineIndex;
    private float3 position;
    private float3 forward;
    private float3 upVector;

    public event Action SplineSamplerValidate;

    private void OnValidate()
    {
        SplineSamplerValidate?.Invoke();
    }

    internal void SampleSplineWidth(float t, float width, out Vector3 p1, out Vector3 p2)
    {
        m_splineContainer.Evaluate(m_splineIndex, t, out position, out forward, out upVector);

        position -= (float3)transform.position;

        float3 right = Vector3.Cross(forward, upVector).normalized;

        p1 = position + (right * width);
        p2 = position + (-right * width);
    }

    internal void SampleSplineWidth(float t, out Vector3 position)
    {
        m_splineContainer.Evaluate(m_splineIndex, t, out this.position, out forward, out upVector);

        this.position -= (float3)transform.position;

        position = this.position;
    }

}
