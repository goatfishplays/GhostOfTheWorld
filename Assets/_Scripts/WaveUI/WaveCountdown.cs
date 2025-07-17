using UnityEngine;
using UnityEngine.Rendering;

public class WaveCountdown : BaseTextUI
{
    protected WaveManager waveManager = null;
    protected bool isBetweenWaves = false;

    void Start()
    {
        waveManager = WaveManager.instance;
        if (waveManager == null)
        {
            Debug.LogWarning("WaveManager does not exist. Deleting WaveCountdown");
            Destroy(gameObject);
        }

        waveManager.startWaveEvent += OnWaveStart;
        waveManager.endWaveEvent += OnWaveEnd;

        UpdateText(defaultText);
    }

    private void FixedUpdate()
    {
        if (isBetweenWaves)
        {
            UpdateText(Mathf.Ceil(waveManager.waveDelayTimer.Time).ToString());
        }
    }

    public void OnWaveStart(int waveNumber)
    {
        isBetweenWaves = false;
        UpdateText(defaultText);
    }

    public void OnWaveEnd()
    {
        isBetweenWaves = true;
    }
}
