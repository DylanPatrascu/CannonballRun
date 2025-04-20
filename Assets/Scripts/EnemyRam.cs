using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;


public class EnemyRam : Enemy
{

    [Header("Spline Settings")]
    [SerializeField] public SplineContainer spline;
    [SerializeField] public float laneOffset = 10f;
    [SerializeField] public bool rightSide = true;
    [SerializeField] public SplineSampler splineSampler;
    [SerializeField] public float roadWidth = 10f;

    private float splineLength;
    public Waypoint entryWaypoint;

    [HideInInspector] public float t = 1f;

    protected override void Start()
    {
        base.Start();

        RamWaypointNavigator navigator = GetComponent<RamWaypointNavigator>();
        if (navigator != null && entryWaypoint != null) {
            navigator.currentWaypoint = entryWaypoint;
        }
        shouldLookAtPlayer = false; // Since this drone only cares about driving
        Debug.Log($"[EnemyRam] Speed set to {moveSpeed} at depth {DifficultyScaler.GetDepth()}");

    }

    public void AssignWaypoint(Waypoint wp)
    {
        entryWaypoint = wp;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            CarDeath death = other.GetComponent<CarDeath>();
            if (death != null) {
                Debug.Log("Hit");
                death.TriggerDeath();
            }
        }
    }

    
}
