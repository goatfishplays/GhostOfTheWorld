using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    float xPos;
    [SerializeField] public float minXPos;
    [SerializeField] public float maxXPos;
    [SerializeField] public float minZPos;
    [SerializeField] public float maxZPos;
    //[SerializeField] int yPos;
    float zPos;
    
    

    public EntityHealth SpawnEnemy(GameObject enemyType)
    {

        xPos = Random.Range(minXPos, maxXPos);
        zPos = Random.Range(minZPos, maxZPos);
        GameObject enemy = Instantiate(enemyType, new Vector3(xPos, -1, zPos), Quaternion.identity);
        
        return enemy.GetComponent<EntityHealth>();

    }
    

}
