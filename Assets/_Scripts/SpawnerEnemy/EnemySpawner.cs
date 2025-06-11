using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject theEnemy;
     int xPos;
    [SerializeField] int yPos;
    [SerializeField] int zPos;
    [SerializeField] int enemyCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    private IEnumerator SpawnEnemy()
    {
        while (enemyCount < 20)
        {
            xPos = Random.Range(10, 11);
            
        }

        // This gives an error if there's no return value.
        return null;
    }
}
