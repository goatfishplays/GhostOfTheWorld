using UnityEngine;

public class EnemiesRemainingUI : BaseTextUI
{
    protected WaveManager waveManager = null;
    void Start()
    {
        waveManager = WaveManager.instance;
        if (waveManager == null)
        {
            Debug.LogWarning("WaveManager does not exist. Deleting EnemiesRemainingUI");
            Destroy(gameObject);
        }


        waveManager.startWaveEvent += OnWaveStart;
        waveManager.enemyDie += OnEnemyDie;
    }

    public void OnWaveStart(int waveNumber)
    {
        UpdateText(defaultText + waveManager.remainEnemy);
    }
    public void OnEnemyDie()
    {
        UpdateText(defaultText + waveManager.remainEnemy);
    }
}
