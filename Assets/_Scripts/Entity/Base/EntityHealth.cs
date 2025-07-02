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
    public event Action<float> OnHit;

    // Misc
    public AudioClip hitSound = null;
    public AudioClip deathSound = null;

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
    /// For health changes
    /// Triggers OnHealthChange Action
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

    /// <summary>
    /// For explicit damage dealing
    /// Triggers OnHit Action
    /// </summary>
    /// <param name="delta"></param> 
    /// <param name="iFramesAddTime"></param>
    /// <param name="ignoresIframes"></param>
    public virtual bool Hit(float delta, float iFramesAddTime = 0.2f, bool ignoresIframes = false)
    {
        if (changingHealth && !dead)
        {
            if (delta > 0 || !hasIFrames || ignoresIframes)
            {
                health += delta;

                if (hitSound != null)
                {
                    AudioManager.instance.PlaySFXAtTracker(hitSound, transform);
                }
                OnHealthChange?.Invoke(delta);
                OnHit?.Invoke(delta);

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
                return true;
            }
        }
        return false;
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
            AudioManager.instance.PlaySFXAtTracker(deathSound, transform);
        }

        dead = true;
        OnDie?.Invoke();
    }
}
