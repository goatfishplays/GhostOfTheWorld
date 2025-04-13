using UnityEngine;

public class Attack : MonoBehaviour
{
    public int ownerId = 0;
    public float attackLifetime = 1000f;
    public float damage = 1f;
    public float iFramesAddTime = 0.2f;
    public bool ignoresIFrames = false;
    public bool destroyOnHit = false;

    private void Awake()
    {
        Destroy(gameObject, attackLifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the object hits a world object and is destroyable.
        if (destroyOnHit == true && ((1 << other.gameObject.layer) & LayerMask.GetMask("World")) != 0)
        {
            Destroy(gameObject);
        }

        Entity entity = other.GetComponent<Entity>();
        // Only deal damage if the entity is not the cause of the attack.
        if (entity != null && ownerId != entity.id)
        {
            // TODO: Make this use a proper hit function instead of directly changing health.
            other.GetComponentInParent<EntityHealth>().ChangeHealth(-damage, iFramesAddTime, ignoresIFrames);

            if (destroyOnHit == true)
            {
                Destroy(gameObject);
            }
        }
    }
}
