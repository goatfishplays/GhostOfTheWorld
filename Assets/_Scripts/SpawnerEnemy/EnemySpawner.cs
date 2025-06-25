using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    public int xPos;
    [SerializeField] int yPos;
    [SerializeField] int zPos;
    
    

    public EntityHealth SpawnEnemy(GameObject enemyType)
    {

        xPos = Random.Range(10, 11);
        zPos = Random.Range(7, 8);
        GameObject enemy = Instantiate(enemyType, new Vector3(xPos, 1, zPos), Quaternion.identity);
        
        return enemy.GetComponent<EntityHealth>();

    }
    

}
