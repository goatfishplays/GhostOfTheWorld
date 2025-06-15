using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class VisualHurtIndicator : MonoBehaviour
{
    public PlayerManager playerManager;
    public float maxWearoffTime = 0.3f;

    private EntityHealth playerHealth;
    private CountdownTimer wearoffTimer;
    private Image hurtIndicator;

    private void Start()
    {
        // Get the player's health
        playerHealth = playerManager.entity.entityHealth;

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
