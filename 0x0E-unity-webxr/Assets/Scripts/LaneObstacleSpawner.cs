using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LaneObstacleSpawner : MonoBehaviour
{
    [Header("Zone and Obstacle Settings")]
    public BoxCollider spawnZone;
    public GameObject obstaclePrefab;
    public int obstacleCount = 4;
    public float minDistanceBetween = 1.5f;

    [Header("Spawn Attempts")]
    public int maxAttempts = 30;

    private List<GameObject> spawnedObstacles = new();

    public void SpawnObstacles()
    {
        if (spawnZone == null || obstaclePrefab == null)
        {
            Debug.LogWarning("SpawnZone or ObstaclePrefab not assigned.");
            return;
        }

        ClearObstacles();
        Vector3 center = spawnZone.bounds.center;
        Vector3 size = spawnZone.bounds.size;

        int spawned = 0;
        int attempts = 0;

        while (spawned < obstacleCount && attempts < maxAttempts)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(center.x - size.x / 2f, center.x + size.x / 2f),
                center.y,
                Random.Range(center.z - size.z / 2f, center.z + size.z / 2f)
            );

            bool tooClose = false;
            foreach (var obj in spawnedObstacles)
            {
                if (Vector3.Distance(randomPos, obj.transform.position) < minDistanceBetween)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                GameObject obs = Instantiate(obstaclePrefab, randomPos, Quaternion.Euler(90f, Quaternion.identity.y, Quaternion.identity.z));
                spawnedObstacles.Add(obs);
                spawned++;
            }

            attempts++;
        }
    }

    public void ClearObstacles()
    {
        foreach (var obj in spawnedObstacles)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedObstacles.Clear();
    }
    public void ClearObstaclesDelayed(float delay = 4f)
    {
        StartCoroutine(ClearObstaclesAfterDelay(delay));
    }
    private IEnumerator ClearObstaclesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearObstacles();
    }
}
