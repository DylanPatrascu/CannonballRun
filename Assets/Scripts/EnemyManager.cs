using System.Collections.Generic;
using System.Collections;
using UnityEngine.Splines;
using Unity.Mathematics;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Testing Options")]
    [SerializeField] private bool spawnProjectileDrones = true;
    [SerializeField] private bool spawnEmpDrone = true;
    [SerializeField] private bool spawnRamDrone = true;

    [Header("Enemy Spawning")]
    [SerializeField] private GameObject empDronePrefab;   // Just one EMP allowed
    [SerializeField] private GameObject[] projectileDronePrefabs;
    [SerializeField] private GameObject ramDronePrefab;
    [SerializeField] private SplineContainer roadSpline;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnDistance = 10f;
    [SerializeField] private float minHeight = 0f;
    [SerializeField] private float maxHeight = 1f;
    [SerializeField] private float horizontalSpacing = 4f;
    [SerializeField] private float ramLaneOffset = 10f;
    private float spawnCooldown = 15f;
    private float spawnTimer = 0f;

    

    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start() {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart() {
        yield return new WaitUntil(() => roadSpline.Splines.Count > 0);

        yield return null;
        yield return null;

        SpawnInitialEnemies();

        if (DifficultyScaler.GetDepth() >= 2 && spawnRamDrone) {
            yield return new WaitForSeconds(5f);
            SpawnRamDrone();
            Debug.Log("[EnemyManager] Forced Ram Drone spawn due to depth ≥ 2");
        }
    }


    private void Update() {
        CleanupDeadEnemies();

        spawnTimer += Time.deltaTime;

        if (activeEnemies.Count < DifficultyScaler.GetEnemyCount() && spawnTimer >= spawnCooldown) {

            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    private bool HasEmpDrone() { return activeEnemies.Exists(e => e != null && e.GetComponent<EnemyEMP>() != null); }


    private void SpawnInitialEnemies() {
        int enemyLimit = DifficultyScaler.GetEnemyCount();
        Debug.Log($"[EnemyManager] Initial spawn limit at depth {DifficultyScaler.GetDepth()}: {enemyLimit}");

        for (int i = 0; i < enemyLimit; i++) {
            SpawnEnemy();
        }
    }


    private void SpawnEnemy() {
        if (player == null) return;

        int slotIndex = GetAvailableSlot();
        if (slotIndex == -1) return;

        GameObject prefabToSpawn = null;

        if (spawnEmpDrone && DifficultyScaler.ShouldAllowEMP() && !HasEmpDrone() && empDronePrefab != null) {
            prefabToSpawn = empDronePrefab;
        } else if (spawnProjectileDrones && DifficultyScaler.ShouldAllowProjectile() && projectileDronePrefabs.Length > 0) {
            prefabToSpawn = projectileDronePrefabs[UnityEngine.Random.Range(0, projectileDronePrefabs.Length)];
        } else if (spawnRamDrone && DifficultyScaler.ShouldAllowRam() && ramDronePrefab != null) {
            prefabToSpawn = ramDronePrefab;
        }

        if (prefabToSpawn == null) return;

        Vector3 offset = GetSlotOffset(slotIndex);
        Vector3 spawnPos = player.position + (player.forward * spawnDistance) + player.right * offset.x + Vector3.up * offset.y;

        GameObject enemy = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        // This debug log will tell what enemy was spawned and what difficulty level it is at
        string enemyType = enemy.GetComponent<Enemy>()?.GetType().Name ?? "Unknown";
        int depth = DifficultyScaler.GetDepth();
        Debug.Log($"[EnemyManager] Spawned: {enemyType} at depth {depth}");

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null) {
            enemyScript.name = $"Enemy_{slotIndex}";
            enemyScript.spawnDistance = spawnDistance;
            enemyScript.SetOffset(player.forward * spawnDistance + player.right * offset.x + Vector3.up * offset.y);
        }

        activeEnemies.Add(enemy);
    }




    private int GetAvailableSlot() {
        for (int i = 0; i < DifficultyScaler.GetEnemyCount(); i++) {

            bool slotTaken = activeEnemies.Exists(e => e != null && e.name == $"Enemy_{i}");
            if (!slotTaken) return i;
        }

        return -1;
    }

    private Vector3 GetSlotOffset(int index) {

        float x = (index - 1.5f) * horizontalSpacing;
        float y = UnityEngine.Random.Range(minHeight, maxHeight);
        return new Vector3(x, y, 0f);
    }

    private void CleanupDeadEnemies() {

        activeEnemies.RemoveAll(e => e == null);
    }

    public void NotifyEnemyDeath(GameObject enemy) {

        if (activeEnemies.Contains(enemy)) {

            activeEnemies.Remove(enemy);

        }
    }
    private void SpawnRamDrone()
    {
        if (ramDronePrefab == null || roadSpline == null) return;

        float t = 1f; // The end of the spline
        SplineSampler sampler = FindFirstObjectByType<SplineSampler>();
        if (sampler == null) return;

        sampler.SampleSplineWidth(t, ramLaneOffset * 2f, out Vector3 p1, out Vector3 p2);

        bool rightSide = UnityEngine.Random.value > 0.5f;
        Vector3 spawnPos = rightSide ? p1 : p2;

        Vector3 center = (p1 + p2) / 2f;
        spawnPos = Vector3.Lerp(spawnPos, center, 0.7f);


        float heightOffset = 1f;
        spawnPos.y = Mathf.Min(p1.y, p2.y) - heightOffset;

        roadSpline.Evaluate(0, Mathf.Clamp01(t - 0.01f), out float3 fwd_pos, out float3 forward_f3, out float3 up_f3);
        Vector3 forward = (Vector3)forward_f3;
        Vector3 up = (Vector3)up_f3;

        Quaternion spawnRot = Quaternion.LookRotation(forward, up) * Quaternion.Euler(0, 180f, 0f);

        GameObject drone = Instantiate(ramDronePrefab, spawnPos, spawnRot);

        EnemyRam script = drone.GetComponent<EnemyRam>();
        if (script != null)
        {
            script.Initialize(roadSpline, sampler, ramLaneOffset * 2f, rightSide, t);
            Debug.Log("[EnemyManager] Called script.Initialize(...)");
        }
    }



}
