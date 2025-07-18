using UnityEngine;

public class WaveNumber : BaseTextUI
{
    protected WaveManager waveManager = null;

    void Start()
    {
        waveManager = WaveManager.instance;
        if (waveManager == null)
        {
            Debug.LogWarning("WaveManager does not exist. Deleting WaveNumber");
            Destroy(gameObject);
        }

        
        waveManager.startWaveEvent += OnWaveStart;
    }

    public void OnWaveStart(int waveNumber)
    {
        UpdateText(defaultText + waveNumber);
    }
}
