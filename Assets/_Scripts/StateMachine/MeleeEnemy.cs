using UnityEngine;

namespace PlatformerAI
{
    public abstract class MeleeEnemy : AttackEnemy
    {
        [Tooltip("The hitbox that activates for basic attacks")]
        [SerializeField] protected GameObject attackHitbox;

        protected override void Start()
        {
            base.Start();

            if (attackHitbox == null)
            {
                Debug.LogWarning("attackHitbox is null");
            }
            else
            {
                attackHitbox.SetActive(false);
            }
        }


    }
}