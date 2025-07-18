using UnityEngine;
using UnityEngine.Rendering;

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
            return;
        }

        waveManager.startWaveEvent += OnWaveStart;
    }

    public void OnWaveStart(int waveNumber)
    {
        // Debug.Log("Wavenumber = " + waveNumber);
        UpdateText(defaultText + waveNumber);
    }
}
