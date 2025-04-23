using UnityEngine;
using System.Collections;
using System;

public class EntityHealth : MonoBehaviour
{
    const float DAMAGE_HIT_SOUND_THRESHHOLD = -1f;

    // Health Variables
    [SerializeField] private bool changingHealth = true;
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float healthChangeRate = 0f;

    // State
    public bool dead = false;
    public bool hasIFrames => co_iFrames != null;

    // Actions/Events
    public event Action OnDie;
    public event Action<float> OnHealthChange;

    // Misc
    public AudioSource hitSound = null;
    public AudioSource deathSound = null;

    // Coroutines
    public Coroutine co_iFrames = null;

    public float GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }


    private void Update()
    {
        // Passive Health Change
        if (changingHealth)
        {
            ChangeHealth(healthChangeRate * Time.deltaTime, 0f, true);
        }
    }

    /// <summary>
    /// TODO Someone remind me to add a separate hit function so the hit sound detection can be less stupid  
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="iFramesAddTime"></param>
    /// <param name="ignoresIframes"></param>
    public virtual void ChangeHealth(float delta, float iFramesAddTime = 0.2f, bool ignoresIframes = false)
    {
        if (changingHealth && !dead)
        {
            if (delta > 0 || !hasIFrames || ignoresIframes)
            {
                health += delta;

                if (hitSound != null && delta < DAMAGE_HIT_SOUND_THRESHHOLD)
                {
                    hitSound.Play();
                }

                OnHealthChange?.Invoke(delta);

                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                else if (health <= 0)
                {
                    Die();
                }


                if (iFramesAddTime > 0f)
                {
                    SetIFrames(iFramesAddTime, overridesCurrent: false);
                }
            }
        }
    }

    public Coroutine SetIFrames(float iFramesSetTime, bool overridesCurrent = false)
    {
        if (hasIFrames)
        {
            if (!overridesCurrent)
            {
                return co_iFrames;
            }
            StopCoroutine(co_iFrames);
        }
        co_iFrames = StartCoroutine(IFrameSet(iFramesSetTime));
        return co_iFrames;
    }


    private IEnumerator IFrameSet(float iFrameAddTime)
    {
        yield return new WaitForSeconds(iFrameAddTime);
        co_iFrames = null;
    }

    public virtual void Die()
    {
        if (deathSound != null)
        {
            deathSound.Play();
        }

        dead = true;
        OnDie?.Invoke();
    }
}
