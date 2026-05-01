using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    public int maxEnemies = 10;

    [Header("Runtime")]
    private List<Transform> spawnPoints = new List<Transform>();
    private int currentEnemies = 0;

    void Start()
    {
        // Cache all children as spawn points
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points found!");
            return;
        }

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentEnemies >= maxEnemies) continue;

            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        currentEnemies++;

        // Track death to reduce count
        var health = enemy.GetComponent<Health>();
        if (health != null)
        {
            health.OnDeath += () =>
            {
                currentEnemies--;
            };
        }
    }
}