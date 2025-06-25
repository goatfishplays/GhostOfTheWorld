using System.Collections.Generic;
using UnityEngine;
using static EnemyEvent;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    
    int currentWave = 0;
    int remainEnemy;
    [SerializeField] private EnemySpawner enemySpawner;
    

    private List<GameObject> introducedEnemies = new();
    public Dictionary<GameObject, int> enemyInLevel= new ();

    
    void HandleEnemyDead()
    {
        remainEnemy--;
        if (remainEnemy == 0)
        {
            Debug.Log("All enemy are dead, start the next wave");
            starNextWave();
        }

    }
    /*void HaddleEnemySpawn()
    {
        remainEnemy++;
    }*/
    void starNextWave()
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
        // set the wave manager to add certain enemy at a certain wave
        foreach (var entry in enemyInLevel)
        {
            if (entry.Value == wave)
                introducedEnemies.Add(entry.Key);
        }

        Dictionary<GameObject, int> spawnPlan = new();
        int totalEnemy = wave + 3;
        for (int i = 0; i<totalEnemy;i++)
        {
            GameObject randomEnemyType = introducedEnemies[Random.Range(0,introducedEnemies.Count)];
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
