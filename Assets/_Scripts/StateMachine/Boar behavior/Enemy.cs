using UnityEngine;
using Utilities;

namespace PlatformerAI
{
    public class Enemy : MeleeEnemy
    {
        [SerializeField] float chargeSpeed = 30f;
        [SerializeField] float chargeDistance = 20f;
        // [SerializeField] float damage = 5f;

        protected override void Start()
        {
            attackState = new EnemyAttackStateBoar(this, agent, PlayerDectector, attackRange, chargeSpeed, chargeDistance, attackCooldown, attackHitbox);
            
            // Run base Start function to prepare State machine.
            base.Start();
        }

        public override void Attack(Entity target)
        {
            // This literally doesn't do anything. idk what to do with it.
            //if (attackTimer.IsRunning) return;
            //attackTimer.Start();
        }

        public override void Jump(Entity target)
        {
            throw new System.NotImplementedException();
        }
    }
}

