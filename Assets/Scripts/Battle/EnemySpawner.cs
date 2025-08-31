using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Header("Normal Enemies")]
    public List<GameObject> enemyPrefabs = new List<GameObject>(); // multiple types

    [Header("Boss Enemies")]
    public List<GameObject> bossPrefabs = new List<GameObject>();  // optional boss list
    public Transform spawnPoint;

    public GameObject SpawnEnemy()
    {
        Vector3 spawnPos = spawnPoint != null
            ? spawnPoint.position
            : Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10f));

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // Make it child of spawnPoint (if exists)
        if (spawnPoint != null)
        {
            enemy.transform.SetParent(spawnPoint, true);   // keep world position
            enemy.transform.localScale = Vector3.one;     // force scale to (1,1,1)
        }

        return enemy;
    }
    
    public GameObject SpawnEnemy(bool spawnBoss = false, int index = -1)
    {
        GameObject prefabToSpawn = null;

        if (spawnBoss && bossPrefabs.Count > 0)
        {
            prefabToSpawn = index >= 0 && index < bossPrefabs.Count
                ? bossPrefabs[index]
                : bossPrefabs[Random.Range(0, bossPrefabs.Count)];
        }
        else if (enemyPrefabs.Count > 0)
        {
            prefabToSpawn = index >= 0 && index < enemyPrefabs.Count
                ? enemyPrefabs[index]
                : enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        }

        if (prefabToSpawn == null)
        {
            Debug.LogWarning("⚠ No valid prefab to spawn!");
            return null;
        }

        // pick spawn position
        Vector3 spawnPos = spawnPoint != null
            ? spawnPoint.position
            : Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10f));

        // instantiate
        GameObject enemy = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        // parent setup
        if (spawnPoint != null)
        {
            enemy.transform.SetParent(spawnPoint, true);
            enemy.transform.localScale = Vector3.one;
        }

        return enemy;
    }
}
