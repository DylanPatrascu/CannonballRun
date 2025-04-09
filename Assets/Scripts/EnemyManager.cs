using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Spawning")]
    [SerializeField] private GameObject empDronePrefab;   // Just one EMP allowed
    [SerializeField] private GameObject[] projectileDronePrefabs; // All others
    [SerializeField] private Transform player;
    [SerializeField] private int maxEnemies = 4;
    [SerializeField] private float spawnDistance = 10f;
    [SerializeField] private float minHeight = 2f;
    [SerializeField] private float maxHeight = 6f;
    [SerializeField] private float horizontalSpacing = 4f;
    

    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start() {
        SpawnInitialEnemies();
    }

    private void Update() {
        CleanupDeadEnemies();

        if (activeEnemies.Count < maxEnemies) {

            SpawnEnemy();
        }
    }

    private bool HasEmpDrone() { return activeEnemies.Exists(e => e != null && e.GetComponent<EnemyEMP>() != null); }


    private void SpawnInitialEnemies() {
        for (int i = 0; i < maxEnemies; i++) {

            SpawnEnemy();
            
        }
    }

    private void SpawnEnemy() {
        if (player == null) return;

        int slotIndex = GetAvailableSlot();
        if (slotIndex == -1) return;

        GameObject prefabToSpawn;

        if (!HasEmpDrone() && empDronePrefab != null) {
            prefabToSpawn = empDronePrefab;
        } else if (projectileDronePrefabs.Length > 0) {
            prefabToSpawn = projectileDronePrefabs[Random.Range(0, projectileDronePrefabs.Length)];
        } else {
            return;
        }

        Vector3 offset = GetSlotOffset(slotIndex);
        Vector3 spawnPos = player.position + (player.forward * spawnDistance) + player.right * offset.x + Vector3.up * offset.y;

        GameObject enemy = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null) {

            enemyScript.name = $"Enemy_{slotIndex}";
            enemyScript.spawnDistance = spawnDistance;
            enemyScript.SetOffset(player.forward * spawnDistance + player.right * offset.x + Vector3.up * offset.y);
        }

        activeEnemies.Add(enemy);
    }



    private int GetAvailableSlot() {
        for (int i = 0; i < maxEnemies; i++) {

            bool slotTaken = activeEnemies.Exists(e => e != null && e.name == $"Enemy_{i}");
            if (!slotTaken) return i;
        }

        return -1;
    }

    private Vector3 GetSlotOffset(int index) {

        float x = (index - 1.5f) * horizontalSpacing;
        float y = Random.Range(minHeight, maxHeight);
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
}
