using JetBrains.Annotations;
using NUnit.Framework.Internal.Filters;
using System;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Attack : MonoBehaviour
{
    public int ownerId = 0;
    public float damage = 1f;
    public bool destroyOnHit = false;

    private void OnTriggerEnter(Collider other)
    {
        // If the object hits an object and is destroyable.
        if (destroyOnHit == true && ((1 << other.gameObject.layer) & LayerMask.GetMask("World")) != 0)
        {
            Destroy(gameObject);
        }

        Entity entity = other.GetComponent<Entity>();
        // Only deal damage if the entity is not the cause of the attack.
        if (entity != null && ownerId != entity.id)
        {
            other.GetComponentInParent<EntityHealth>().ChangeHealth(-damage);

            if (destroyOnHit == true)
            {
                Destroy(gameObject);
            }
        }
        
    }
}
