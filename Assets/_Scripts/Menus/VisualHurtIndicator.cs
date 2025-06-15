using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class VisualHurtIndicator : MonoBehaviour
{
    [Tooltip("How long the red flash appears after getting hit")]
    public float maxWearoffTime = 0.3f;

    private EntityHealth playerHealth;
    private CountdownTimer wearoffTimer;
    private Image hurtIndicator;

    private void Start()
    {
        // Get the player's health
        playerHealth = PlayerManager.instance.entity.entityHealth;
        if (playerHealth == null)
        {
            Debug.LogWarning("playerHealth is null. Disabling VisualHurtIndicator");
            gameObject.SetActive(false);
        }

        // Disable hurt indicator image
        hurtIndicator = gameObject.GetComponent<Image>();
        hurtIndicator.enabled = false;

        playerHealth.OnHit += turnOnHurtIndicator;

        // Run turnOffHurtIndicator after getting hit
        wearoffTimer = new CountdownTimer(maxWearoffTime);
        wearoffTimer.OnTimerStop += turnOffHurtIndicator;
    }

    private void OnEnable()
    {
        playerHealth.OnHit += turnOnHurtIndicator;
        wearoffTimer.OnTimerStop += turnOffHurtIndicator;
    }

    private void OnDisable()
    {
        playerHealth.OnHit -= turnOnHurtIndicator;
        wearoffTimer.OnTimerStop -= turnOffHurtIndicator;
    }

    private void OnDestroy()
    {
        playerHealth.OnHit -= turnOnHurtIndicator;
        wearoffTimer.OnTimerStop -= turnOffHurtIndicator;
    }

    private void turnOnHurtIndicator(float delta)
    {
        hurtIndicator.enabled = true;
        wearoffTimer.Start();
    }

    private void turnOffHurtIndicator()
    {
        hurtIndicator.enabled = false;
        wearoffTimer.Stop();
    }

    // Update is called once per frame
    private void Update()
    {
        wearoffTimer.Tick(Time.deltaTime);
    }
}
