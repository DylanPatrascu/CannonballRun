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
    [SerializeField] private TrafficWaypoints trafficWaypoints;

    private float spawnCooldown = 15f;
    private float spawnTimer = 0f;

    

    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start() {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart() {
        yield return new WaitUntil(() => roadSpline.Splines.Count > 0);


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
        if (ramDronePrefab == null || trafficWaypoints == null) {
            Debug.LogError("[EnemyManager] Missing ramDronePrefab or trafficWaypoints reference!");
            return;
        }

        int laneIndex = UnityEngine.Random.value > 0.5f ? 2 : 4;
        Transform lane = trafficWaypoints.GetLaneTransform(laneIndex);

        if (lane == null || lane.childCount == 0)
        {
            Debug.LogError($"[EnemyManager] Lane {laneIndex} has no waypoints!");
            return;
        }

        Waypoint entryWaypoint = lane.GetChild(lane.childCount - 1).GetComponent<Waypoint>();
        GameObject drone = Instantiate(ramDronePrefab, entryWaypoint.transform.position, Quaternion.identity);

        EnemyRam ram = drone.GetComponent<EnemyRam>();
        if (ram != null)
        {
            ram.AssignWaypoint(entryWaypoint);
            Debug.Log("[EnemyManager] Ram Drone spawned using waypoints.");
        }
    }

}
