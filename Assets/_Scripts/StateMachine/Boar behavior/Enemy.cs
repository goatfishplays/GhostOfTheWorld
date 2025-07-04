using Unity.VisualScripting;
using UnityEngine;
using Utilities;

namespace PlatformerAI
{
    public class Enemy : MeleeEnemy
    {
        [SerializeField] protected BoarSO boarSO = null;
        
        protected override void Start()
        {
            attackEnemySO = boarSO;
            attackState = new EnemyAttackStateBoar(this, agent, PlayerDectector, boarSO, attackHitbox);
            
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

