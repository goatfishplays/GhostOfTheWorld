using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerEntityHealth : EntityHealth
{
    public GameObject visualHurtIndicator;
    private float hurtTimer = 0;
    public const float maxHurtTime = 0.3f;
    private bool timerActive = false;

    public override void onHit(float damage, float iFramesAddTime = 0.2f, bool ignoresIframes = false)
    {
        visualHurtIndicator.SetActive(true);
        timerActive = true;

        base.onHit(damage, iFramesAddTime, ignoresIframes);
    }

    public void Update()
    {
        if (timerActive)
        {
            hurtTimer += Time.deltaTime;
        }

        if (hurtTimer >= maxHurtTime)
        {
            visualHurtIndicator.SetActive(false);
            hurtTimer = 0;
            timerActive = false;
        }
    }


}
