using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;   
    public Transform spawnPoint;     

    public void SpawnEnemy()
    {
        Vector3 spawnPos;

        if (spawnPoint != null)
        {
            spawnPos = spawnPoint.position;
        }
        else
        {
            spawnPos = Camera.main.ScreenToWorldPoint(
                new Vector3(Screen.width / 2, Screen.height / 2, 10f) 
            );
        }

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
