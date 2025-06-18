using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject theEnemy;
    public int xPos;
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
        while (enemyCount < 5)
        {
            xPos = Random.Range(10, 11);
            zPos = Random.Range(7, 8);
            Instantiate(theEnemy, new Vector3(xPos, 1, zPos), Quaternion.identity);
            yield return new WaitForSeconds(1);
            enemyCount++;
        }
    }

}
