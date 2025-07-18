using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    protected float xMaxSpawnDist = 1f;
    protected float yMaxSpawnDist = 1f;
    protected float zMaxSpawnDist = 1f;
    
    [Tooltip("The transform that determines the spawn radius based on it's scale")]
    public Transform enemySpawnZone = null;
    [Tooltip("The transform that all enemies will be children of")]
    public Transform parent = null;

    // This causes a bug because the enemies spawn before spawn area gets set. This causes all the enemies to spawn in the same spot at the beginning. idk feature not a bug for now
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
        yMaxSpawnDist = enemySpawnZone.transform.localScale.y / 2;
        zMaxSpawnDist = enemySpawnZone.transform.localScale.z / 2;
    }

    public EntityHealth SpawnEnemy(GameObject enemyType)
    {
        Vector3 spawnPos = new Vector3(Random.Range(-xMaxSpawnDist, xMaxSpawnDist), Random.Range(-yMaxSpawnDist, yMaxSpawnDist), Random.Range(-zMaxSpawnDist, zMaxSpawnDist)) + enemySpawnZone.position;
        GameObject enemy = Instantiate(enemyType, spawnPos, Quaternion.identity, parent);

        return enemy.GetComponent<EntityHealth>();
    }



}
