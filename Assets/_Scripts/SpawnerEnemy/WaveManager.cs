using System.Collections.Generic;
using UnityEngine;
using static EnemyEvent;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    
    public int currentWave = 0;
    public int remainEnemy;
    public EnemySpawner enemySpawner;
    [System.Serializable]
    struct EnemyInfo
    {
        public int wave;
        public GameObject prefab;
    }

    [SerializeField] private EnemyInfo[] enemyInLevel;

    List<GameObject> introducedEnemies = new ();

    private void Start()
    {
        StartNextWave();
    }

    void HandleEnemyDead()
    {
        remainEnemy--;
        if (remainEnemy == 0)
        {
            Debug.Log("All enemy are dead, start the next wave");
            StartNextWave();
        }

    }
    /*void HaddleEnemySpawn()
    {
        remainEnemy++;
    }*/
    void StartNextWave()
    {
        currentWave++;
        Dictionary<GameObject, int> plan = GetSpawnPlan(currentWave);
        remainEnemy = plan.Values.Sum();
        SpawnWave(plan);

    }
    //make a random spawn
    //make a function that return a dictionary to determine which type of spawning enemy
    Dictionary<GameObject, int> GetSpawnPlan(int wave)
    {
        Debug.Log(wave);
        
        
        foreach (var entry in enemyInLevel)
        {
            if (entry.wave == wave)
                introducedEnemies.Add(entry.prefab);
        }
        
        Dictionary<GameObject, int> spawnPlan = new();
        int totalEnemy = wave + 3;
        for (int i = 0; i<totalEnemy;i++)
        {

            GameObject randomEnemyType = introducedEnemies[Random.Range(0, introducedEnemies.Count)];
 
            if (!spawnPlan.ContainsKey(randomEnemyType))
                spawnPlan[randomEnemyType] = 0;
            spawnPlan[randomEnemyType]++;
        }
        return spawnPlan;
    }
    // spawn the wave function, 
    private void SpawnWave(Dictionary<GameObject, int> plan)
    {
        foreach (var entry in plan)
        {
            for (int i = 0; i < entry.Value; i++)
            {
                EntityHealth temp = enemySpawner.SpawnEnemy(entry.Key);
                temp.OnDie += HandleEnemyDead;
                
            }
        }
    }

    
   
}
