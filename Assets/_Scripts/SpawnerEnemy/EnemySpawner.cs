using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    public float minXPos;
    public float maxXPos;
    public float minZPos;
    public float maxZPos;
    
    [Tooltip("The Vector3 that is the center of the spawn radius")]
    public Transform center = null;
    [Tooltip("The transform that all enemies will be children of")]
    public Transform parent = null;

    public void Start()
    {
        if (center == null)
        {
            center = gameObject.transform;
        }
        if (parent == null)
        {
            parent = gameObject.transform;
        }
    }

    public EntityHealth SpawnEnemy(GameObject enemyType)
    {
        Vector3 spawnPos = new Vector3(Random.Range(minXPos, maxXPos), 0, Random.Range(minZPos, maxZPos)) + center.position;
        GameObject enemy = Instantiate(enemyType, spawnPos, Quaternion.identity, parent);
        
        return enemy.GetComponent<EntityHealth>();

    }
    

}
