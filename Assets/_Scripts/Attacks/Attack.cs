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

    // Note: This only triggers on trigger enter so if the player is in the hitbox it will only hit once.
    private void OnTriggerEnter(Collider other)
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
            OnAttackHit?.Invoke();
            OnEntityHit?.Invoke(entity);
            other.GetComponentInParent<EntityHealth>().Hit(-atttakConfig.damage, atttakConfig.iFramesAddTime, atttakConfig.ignoresIFrames);

            if (atttakConfig.destroyOnHit == true)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        OnAttackDestroy?.Invoke();
    }
}
