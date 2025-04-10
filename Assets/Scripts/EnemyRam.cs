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

    [HideInInspector] public float t = 1f;

    protected override void Start()
    {
        base.Start();
        shouldLookAtPlayer = false; // Since this drone only cares about driving
        splineLength = spline.CalculateLength();

    }

    protected override void Update()
    {
        if (splineSampler == null || isDead) return;
        base.Update();

        // Move toward the start of the spline
        t -= (moveSpeed * Time.deltaTime) / splineLength;

        if (t < 0f)
        {
            Destroy(gameObject);
            return;
        }

        // Sample the left and right edges of the road
        Vector3 p1, p2;
        splineSampler.SampleSplineWidth(t, roadWidth, out p1, out p2);

        // Blend toward center to keep drone closer to middle
        Vector3 center = (p1 + p2) / 2f;
        Vector3 rawTarget = rightSide ? p1 : p2;
        Vector3 targetPos = Vector3.Lerp(rawTarget, center, 0.7f); // 0.7 = how close to center
        transform.position = targetPos;

        // Look ahead slightly for smoother rotation
        float futureT = Mathf.Clamp01(t - 0.01f);
        Vector3 fwd_p1, fwd_p2;
        splineSampler.SampleSplineWidth(futureT, roadWidth, out fwd_p1, out fwd_p2);

        Vector3 fwd_center = (fwd_p1 + fwd_p2) / 2f;
        Vector3 rawFwd = rightSide ? fwd_p1 : fwd_p2;
        Vector3 forwardDir = (Vector3.Lerp(rawFwd, fwd_center, 0.7f) - targetPos).normalized;

        transform.rotation = Quaternion.LookRotation(forwardDir, Vector3.up) * Quaternion.Euler(0f, 180f, 0f);
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
