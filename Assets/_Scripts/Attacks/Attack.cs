using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int ownerId = 0;
    public AttackConfig atttakConfig;
    public event Action OnAttackHits;
    public event Action OnAttackDestroyed;

    private void Awake()
    {
        if (atttakConfig.attackLifetime != -1f)
        {
            Destroy(gameObject, atttakConfig.attackLifetime);
        }
    }

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
            OnAttackHits?.Invoke();
            other.GetComponentInParent<EntityHealth>().Hit(-atttakConfig.damage, atttakConfig.iFramesAddTime, atttakConfig.ignoresIFrames);

            if (atttakConfig.destroyOnHit == true)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        OnAttackDestroyed?.Invoke();
    }
}
