using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private SplineContainer spline;
    [SerializeField] private SplineSampler splineSampler;
    [SerializeField] private GameObject[] buildingPrefabs;

    [Header("Placement Settings")]
    [SerializeField] private float spacing = 15f;
    [SerializeField] private float offsetFromRoad = 20f;


    // This is a variable to store the half extents of the building's collider
    private Vector3 buildingHalfExtents;

    private void Start()
    {
        if (spline == null || splineSampler == null || buildingPrefabs == null || buildingPrefabs.Length == 0)
        {

            Debug.LogWarning("[EnvironmentManager] Spline, SplineSampler, or building prefab are/is not assigned");
            return;
        }

        float splineLength = spline.Splines[0].GetLength();
        Debug.Log("[EnvironmentManager] Spline length: " + splineLength);

        int counter = 0;
        // This will loop through the spline along world distance
        for (float d = 0f; d < splineLength; d += spacing)
        {

            float t = d / splineLength;

            Vector3 p1, p2;
            // Here we multiply offsetFromRoad by 2f so that our sample covers a reasonable width
            splineSampler.SampleSplineWidth(t, offsetFromRoad * 2f, out p1, out p2);
            Vector3 center = (p1 + p2) / 2f;

            for (int s = -1; s <= 1; s += 2)
            {
                Vector3 rawPos = (s == 1) ? p1 : p2;
                Vector3 spawnPos = Vector3.Lerp(rawPos, center, 0.5f);

                float futureT = Mathf.Clamp01(t + 0.01f);
                Vector3 fwd_p1, fwd_p2;
                splineSampler.SampleSplineWidth(futureT, offsetFromRoad * 2f, out fwd_p1, out fwd_p2);


                Vector3 forwardPoint = (s == 1) ? fwd_p1 : fwd_p2;
                Vector3 forwardDir = (forwardPoint - spawnPos).normalized;

                Quaternion rotation = Quaternion.identity;
                if (forwardDir != Vector3.zero)
                {
                    rotation = Quaternion.LookRotation(forwardDir, Vector3.up);
                }

                GameObject chosenPrefab = buildingPrefabs[UnityEngine.Random.Range(0, buildingPrefabs.Length)];
                GameObject preview = Instantiate(chosenPrefab, spawnPos, rotation);
                preview.SetActive(false);

                // Computes total bounds from all renderers (including children)
                Bounds bounds = new Bounds(spawnPos, Vector3.zero);
                Renderer[] renderers = preview.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                {
                    bounds.Encapsulate(r.bounds);
                }

                // Checks overlap using bounds size
                Collider[] colliders = Physics.OverlapBox(
                    bounds.center,
                    bounds.extents,
                    rotation
                );

                bool overlapsRoad = false;
                foreach (Collider col in colliders)
                {
                    if (col.CompareTag("Road"))
                    {
                        overlapsRoad = true;
                        break;
                    }
                }

                Destroy(preview);

                if (overlapsRoad)
                {
                    Debug.Log($"[EnvironmentManager] Skipped building at t={t:F2} (road overlap)");
                    continue;
                }

                Instantiate(chosenPrefab, spawnPos, rotation, transform);
                counter++;
            }
        }

        Debug.Log("[EnvironmentManager] Placed " + counter + " buildings.");
    }
}
