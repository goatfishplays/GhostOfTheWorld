using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

namespace PlatformerAI
{
    public class WolfEnemy : MeleeEnemy
    {
        [SerializeField] protected WolfSO wolfSO = null;

        protected CountdownTimer jumpTimer;

        protected override void Start()
        {
            attackEnemySO = wolfSO;
            if (wolfSO.jumpRangeMin >= wolfSO.jumpRangeMax)
            {
                Debug.LogWarning("Wolf: jumpRangeMin is greater than or equal to jumpRangeMax");
            }
            if (wolfSO.jumpTimeLength <= 0)
            {
                Debug.LogWarning("Wolf: jumpTimeLength is less than or equal to 0");
            }

            
            attackState = new EnemyAttackStateWolf(this, agent, PlayerDectector, wolfSO.attackRange, attackHitbox);

            // Run base Start function to prepare State machine.
            base.Start();

            jumpTimer = new CountdownTimer(wolfSO.jumpCooldown);
      
            
            var jumpAttackState = new EnemyJumpAttackWolf(
                this,
                agent,
                PlayerDectector,
                wolfSO
            );

            // chase → jump‐attack. When distance to player within min and max jump range and not on cooldown
            At(chaseState, jumpAttackState, new FuncPredicated(() => {
                var player = PlayerDectector.GetPlayer();
                float dist = Vector3.Distance(transform.position, player.position);
                return dist <= wolfSO.jumpRangeMax && dist >= wolfSO.jumpRangeMin && !jumpTimer.IsRunning;
            }));

            // ★ jump‐attack → chase, when distance to player not within min and max jump range
            // Note: The wolf will leave jump state part way through the jump because it leaves jump range.
            At(jumpAttackState, chaseState, new FuncPredicated(() =>
            {
                var player = PlayerDectector.GetPlayer();
                float dist = Vector3.Distance(transform.position, player.position);
                // add one condition for jump
                return dist > wolfSO.jumpRangeMax || dist < wolfSO.jumpRangeMin || jumpTimer.IsRunning;
            }));
        }

        protected override void Update()
        {
            base.Update();
            // attackTimer.Tick(Time.deltaTime);
            jumpTimer.Tick(Time.deltaTime);
        }

        public override void Attack(Entity target)
        {

          
        }

        public override void Jump(Entity target)
        {
            if (jumpTimer.IsRunning) return;
            jumpTimer.Start();
            //Debug.Log("Wolf Jump Attack");
        }
    }
}
