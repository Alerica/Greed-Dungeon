using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpawner : MonoBehaviour
{
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private float strikeHeight = 2f; // optional extra height offset
    [SerializeField] private float chainDelay = 0.2f; // delay between strikes

    public void CastLightning()
    {
        StartCoroutine(SpawnLightningChain());
    }

    private IEnumerator SpawnLightningChain()
    {
        // Collect all GameObjects on layer 6 (Enemy layer)
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID);
        List<Transform> enemies = new List<Transform>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == 6) // Enemy layer
                enemies.Add(obj.transform);
        }

        if (enemies.Count == 0) yield break;

        // Get world position of screen center
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane + 10f);
        Vector3 worldCenter = Camera.main.ViewportToWorldPoint(screenCenter);

        // First strike: from screen center into enemy[0]
        LightningBolt firstBolt = Instantiate(lightningPrefab, Vector3.zero, Quaternion.identity)
            .GetComponent<LightningBolt>();

        if (firstBolt.getStartPoint() && firstBolt.getEndPoint())
        {
            Vector3 startPos = worldCenter + Vector3.up * strikeHeight;
            firstBolt.getStartPoint().position = startPos;
            firstBolt.getEndPoint().position = enemies[0].position;
        }

        // wait before chaining
        yield return new WaitForSeconds(chainDelay);

        // Chain to next enemies
        for (int i = 1; i < enemies.Count; i++)
        {
            LightningBolt bolt = Instantiate(lightningPrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<LightningBolt>();

            if (bolt.getStartPoint() && bolt.getEndPoint())
            {
                bolt.getStartPoint().position = enemies[i - 1].position;
                bolt.getEndPoint().position = enemies[i].position;
            }

            yield return new WaitForSeconds(chainDelay);
        }
    }
}
