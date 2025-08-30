using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;   
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
}
