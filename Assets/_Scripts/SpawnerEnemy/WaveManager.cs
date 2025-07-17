using System.Collections.Generic;
using UnityEngine;
using static EnemyEvent;
using System.Linq;
using System.Collections;
using System;
using Utilities;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance = null;

    public int currentWave = 0;
    public int remainEnemy;
    public float timeBetweenWaves = 5f;
    public EnemySpawner enemySpawner;
    public CountdownTimer waveDelayTimer;

    public event Action<int> startWaveEvent;
    public event Action enemyDie;// for whenever enemy die, 
    public event Action endWaveEvent;

    [System.Serializable]
    struct EnemyInfo
    {
        public int wave;
        public GameObject prefab;
    }

    [SerializeField] private EnemyInfo[] enemyInLevel;

    List<GameObject> introducedEnemies = new ();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple WaveManagers Detected Deleting Second");
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        waveDelayTimer = new CountdownTimer(timeBetweenWaves);
        waveDelayTimer.OnTimerStop += StartNextWave;

        startWaveEvent += pickupAllEnemyDrops;
        StartNextWave();
    }

    private void Update()
    {
        waveDelayTimer.Tick(Time.deltaTime);
    }

    void HandleEnemyDead()
    {
        remainEnemy--;
        enemyDie?.Invoke();
        if (remainEnemy == 0)
        {
            Debug.Log("All enemy are dead, start the next wave");
            waveDelayTimer.Start();
            endWaveEvent?.Invoke();
        }

    }
    
    void StartNextWave()
    {
        currentWave++;

        Dictionary<GameObject, int> plan = GetSpawnPlan(currentWave);

        remainEnemy = plan.Values.Sum();
        SpawnWave(plan);

        startWaveEvent?.Invoke(currentWave);
    }

    void pickupAllEnemyDrops(int waveCount)
    {
        DropManager.instance.PickupAllDrops(PlayerManager.instance.entity);
    }

    IEnumerator delayWave(float delayTime)
    {

        yield return new WaitForSeconds(delayTime);
        StartNextWave();
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

            GameObject randomEnemyType = introducedEnemies[UnityEngine.Random.Range(0, introducedEnemies.Count)];
 
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
