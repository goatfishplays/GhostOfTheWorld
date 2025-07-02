using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int ownerId = 0;
    public AttackConfig atttakConfig;
    public event Action OnAttackHit;
    public event Action<Entity> OnEntityHit;
    public event Action OnAttackDestroy;

    private void Awake()
    {
        if (atttakConfig.attackLifetime != -1f)
        {
            Destroy(gameObject, atttakConfig.attackLifetime);
        }
    }

    // This may be very inefficient and not good.
    // Triggers every second and calls the hit function.
    private void OnTriggerStay(Collider other)
    {
        // If the object hits a world object and is destroyable.
        if (atttakConfig.destroyOnHit == true && ((1 << other.gameObject.layer) & LayerMask.GetMask("World")) != 0)
        {
            Destroy(gameObject);
        }

        Entity entity = other.GetComponent<Entity>();
        // Only deal damage if the entity is not the cause of the attack.
        if (entity != null && ownerId != entity.id)
        {
            bool hasHit = other.GetComponentInParent<EntityHealth>().Hit(-atttakConfig.damage, atttakConfig.iFramesAddTime, atttakConfig.ignoresIFrames);

            if (hasHit)
            {
                OnAttackHit?.Invoke();
                OnEntityHit?.Invoke(entity);
                if (atttakConfig.destroyOnHit == true)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnDestroy()
    {
        OnAttackDestroy?.Invoke();
    }
}
