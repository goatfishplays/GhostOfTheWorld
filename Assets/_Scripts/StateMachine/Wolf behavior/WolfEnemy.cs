using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

namespace PlatformerAI
{
    public class WolfEnemy : MeleeEnemy
    {
        [Tooltip("Min range the wolf can start a jump from.")]
        [SerializeField] protected float jumpRangeMin = 10f;
        [Tooltip("Max range the wolf can start a jump from. Should be smaller than detection radius.")]
        [SerializeField] protected float jumpRangeMax = 15f;
        [Tooltip("Number of seconds the wolf takes to complete a jump.")]
        [SerializeField] protected float jumpTimeLength = 2f;
        [Tooltip("Time in seconds between wolf jumping. Cooldown starts the moment they start jumping.")]
        [SerializeField] protected float jumpCooldown = 5f;
        [Tooltip("Curve the wolf follows when jumping. Time is adjusted to the jumpTimeLength.")]
        [SerializeField] protected AnimationCurve HeightCurve;

        protected CountdownTimer jumpTimer;

        protected override void Start()
        {
            if (jumpRangeMin >= jumpRangeMax)
            {
                Debug.LogWarning("Wolf: jumpRangeMin is greater than or equal to jumpRangeMax");
            }
            if (jumpTimeLength <= 0)
            {
                Debug.LogWarning("Wolf: jumpTimeLength is less than or equal to 0");
            }

            
            attackState = new EnemyAttackStateWolf(this, agent, PlayerDectector, attackRange, attackHitbox);

            // Run base Start function to prepare State machine.
            base.Start();

            jumpTimer = new CountdownTimer(jumpCooldown);
      
            
            var jumpAttackState = new EnemyJumpAttackWolf(
                this,
                agent,
                PlayerDectector,
                HeightCurve,
                jumpTimeLength,
                jumpCooldown
                
            );

            // chase → jump‐attack. When distance to player within min and max jump range and not on cooldown
            At(chaseState, jumpAttackState, new FuncPredicated(() => {
                var player = PlayerDectector.GetPlayer();
                float dist = Vector3.Distance(transform.position, player.position);
                return dist <= jumpRangeMax && dist >= jumpRangeMin && !jumpTimer.IsRunning;
            }));

            // ★ jump‐attack → chase, when distance to player not within min and max jump range
            // Note: The wolf will leave jump state part way through the jump because it leaves jump range.
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
            //Debug.Log("Wolf Attack");
        }

        public override void Jump(Entity target)
        {
            if (jumpTimer.IsRunning) return;
            jumpTimer.Start();
            //Debug.Log("Wolf Jump Attack");
        }
    }
}
