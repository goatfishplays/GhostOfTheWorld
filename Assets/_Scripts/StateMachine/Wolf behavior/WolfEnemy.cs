using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

namespace PlatformerAI
{
    public class WolfEnemy : MeleeEnemy
    {
        [SerializeField] protected float jumpRangeMax = 15f;
        [SerializeField] protected float jumpRangeMin = 10f;
        [Tooltip("Number of seconds the wolf takes to complete a jump")]
        [SerializeField] protected float jumpTimeLength = 2f;
        [SerializeField] protected float jumpCooldown = 5f;
        [SerializeField] protected AnimationCurve HeightCurve;
        [SerializeField] protected CountdownTimer jumpTimer;

        protected override void Start()
        {
            if (jumpRangeMin >= jumpRangeMax)
            {
                Debug.LogWarning("Wolf: jumpRangeMin is greater than or equal to jumpRangeMax");
            }
            
            attackState = new EnemyAttackStateWolf(this, agent, PlayerDectector, attackRange, attackHitbox);

            // Run base Start function to prepare State machine.
            base.Start();

            jumpTimer = new CountdownTimer(jumpCooldown);
      
            // pass jumpSpeed _then_ jumpCooldown
            var jumpAttackState = new EnemyJumpAttackWolf(
                this,
                agent,
                PlayerDectector,
                HeightCurve,
                jumpRangeMax,
                jumpRangeMin,
                jumpTimeLength,
                jumpCooldown
                
            );
            //// Enter jump attack when outside of attack range but within jump range
            //At(attackState, jumpAttackState, new FuncPredicated(() => {
            //    var player = PlayerDectector.GetPlayer();
            //    float dist = Vector3.Distance(transform.position, player.position);
            //    return dist > attackRange && dist <= jumpRange;

            //}));

            // chase → jump‐attack. When distance to player within min and max jump range and not on cooldown
            At(chaseState, jumpAttackState, new FuncPredicated(() => {
                var player = PlayerDectector.GetPlayer();
                float dist = Vector3.Distance(transform.position, player.position);
                return dist <= jumpRangeMax && dist >= jumpRangeMin && !jumpTimer.IsRunning;
            }));

            //// jump‐attack → melee‐attack
            //At(jumpAttackState, attackState, new FuncPredicated(() => {
            //    var player = PlayerDectector.GetPlayer();
            //    return Vector3.Distance(transform.position, player.position) <= attackRange;
            //}));

            // ★ jump‐attack → chase, when distance to player not within min and max jump range
            At(jumpAttackState, chaseState, new FuncPredicated(() =>
            {
                var player = PlayerDectector.GetPlayer();
                float dist = Vector3.Distance(transform.position, player.position);
                return dist > jumpRangeMax || dist < jumpRangeMin;

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

            //if (attackTimer.IsRunning) return;
            //attackTimer.Start();
            Debug.Log("Wolf Attack");
        }
        public override void Jump(Entity target)
        {
            if (jumpTimer.IsRunning) return;
            jumpTimer.Start();
            Debug.Log("Wolf Jump Attack");
        }
    }
}
