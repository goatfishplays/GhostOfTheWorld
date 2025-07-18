using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    protected float xMaxSpawnDist = 1f;
    protected float zMaxSpawnDist = 1f;
    
    [Tooltip("The transform that determines the spawn radius based on it's scale")]
    public Transform enemySpawnZone = null;
    [Tooltip("The transform that all enemies will be children of")]
    public Transform parent = null;

    public void Start()
    {
        if (enemySpawnZone == null)
        {
            enemySpawnZone = gameObject.transform;
        }
        if (parent == null)
        {
            parent = gameObject.transform;
        }

        xMaxSpawnDist = enemySpawnZone.transform.localScale.x / 2;
        zMaxSpawnDist = enemySpawnZone.transform.localScale.y / 2;
    }

    public EntityHealth SpawnEnemy(GameObject enemyType)
    {
        const int maxAttempts = 100000000;
        Vector3 spawnPos = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        Vector3 checkSize = new Vector3(0.8f, 1f, 0.8f); // Adjust based on enemy size
        bool positionFound = false;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float randX = Random.Range(-xMaxSpawnDist, xMaxSpawnDist);
            float randZ = Random.Range(-zMaxSpawnDist, zMaxSpawnDist);
            spawnPos = enemySpawnZone.position + new Vector3(randX, 0f, randZ);

            // Check if area is clear (no colliders overlapping)
            if (!Physics.CheckBox(spawnPos + Vector3.up, checkSize))
            {
                positionFound = true;
                break;
            }
        }

        if (!positionFound)
        {
            Debug.LogWarning("Could not find valid spawn location after multiple attempts.");
            return null;
        }

        GameObject enemy = Instantiate(enemyType, spawnPos, rotation, parent);
        return enemy.GetComponent<EntityHealth>();

    }
    

}
