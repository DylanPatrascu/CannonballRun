using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TrafficWaypoints : MonoBehaviour
{
    [Header("Lanes")]
    [SerializeField] private Transform lane1;
    [SerializeField] private Transform lane2;
    [SerializeField] private Transform lane3;
    [SerializeField] private Transform lane4;

    [Header("Spline Sampler")]
    [SerializeField] private SplineSampler sampler;

    [Header("Lane Distance")]
    [SerializeField] private float innerLaneDistace = 5;
    [SerializeField] private float outerLaneDistace = 15;

    List<Waypoint> lane1Waypoints;
    List<Waypoint> lane2Waypoints;
    List<Waypoint> lane3Waypoints;
    List<Waypoint> lane4Waypoints;

    Vector3 p1;
    Vector3 p2;

    private void Start()
    {
        
    }

    public void SetWaypoints()
    {
        lane1Waypoints = new List<Waypoint>();
        lane2Waypoints = new List<Waypoint>();
        lane3Waypoints = new List<Waypoint>();
        lane4Waypoints = new List<Waypoint>();
        foreach (Transform child in lane1)
        {
            lane1Waypoints.Add(child.GetComponent<Waypoint>());
        }
        foreach (Transform child in lane2)
        {
            lane2Waypoints.Add(child.GetComponent<Waypoint>());
        }
        foreach (Transform child in lane3)
        {
            lane3Waypoints.Add(child.GetComponent<Waypoint>());
        }
        foreach (Transform child in lane4)
        {
            lane4Waypoints.Add(child.GetComponent<Waypoint>());
        }

        float t;
        Debug.Log("Here");
        for (int i = 0; i < lane1Waypoints.Count; i++)
        {
            t = (i + 1f) / ((float)lane1Waypoints.Count + 1f);
            Debug.Log(t);
            sampler.SampleSplineWidth(t, innerLaneDistace, out p1, out p2);
            lane1Waypoints[i].transform.position = p1;
            sampler.SampleSplineWidth(1 - t, innerLaneDistace, out p1, out p2);
            lane3Waypoints[i].transform.position = p2;
            sampler.SampleSplineWidth(t, outerLaneDistace, out p1, out p2);
            lane2Waypoints[i].transform.position = p1;
            sampler.SampleSplineWidth(1 - t, outerLaneDistace, out p1, out p2);
            lane4Waypoints[i].transform.position = p2;
        }
    }

    public Transform GetLaneTransform(int laneIndex)
    {
        switch (laneIndex) {

            
            case 1: return lane1;
            case 2: return lane2;
            case 3: return lane3;
            case 4: return lane4;
            default:
                Debug.LogError("[TrafficWaypoints] Invalid lane index.");
                return null;
        }
    }

}