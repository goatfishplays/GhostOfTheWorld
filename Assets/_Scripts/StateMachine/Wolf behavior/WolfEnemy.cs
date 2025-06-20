using UnityEngine;
using Utilities;

namespace PlatformerAI
{
    public class WolfEnemy : MeleeEnemy
    {
        [SerializeField] float jumpRange = 10f;
        [SerializeField] float jumpCooldown = 5f;
        [SerializeField] AnimationCurve HeightCurve;
        [SerializeField] CountdownTimer jumpTimer;

        protected override void Start()
        {
            attackState = new EnemyAttackStateWolf(this, agent, PlayerDectector, attackRange, attackHitbox);
            Debug.Log("Done with attack");

            // Run base Start function to prepare State machine.
            base.Start();

            jumpTimer = new CountdownTimer(jumpCooldown);
      
            // pass jumpSpeed _then_ jumpCooldown
            var jumpAttackState = new EnemyJumpAttackWolf(
                this,
                agent,
                PlayerDectector,
                HeightCurve,
                jumpRange
                
            );

            Debug.Log("Done with base start");
            // Enter jump attack when outside of attack range but within jump range
            At(attackState, jumpAttackState, new FuncPredicated(() => {
                var player = PlayerDectector.GetPlayer();
                float dist = Vector3.Distance(transform.position, player.position);
                return dist > attackRange && dist <= jumpRange;

            }));

            // chase → jump‐attack
            At(chaseState, jumpAttackState, new FuncPredicated(() => {
                var player = PlayerDectector.GetPlayer();
                float dist = Vector3.Distance(transform.position, player.position);
                return dist <= jumpRange
                    && dist > attackRange
                    ;       // only if our cooldown is ready
            }));

            Debug.Log("Done with into jump attack");

            // jump‐attack → melee‐attack
            At(jumpAttackState, attackState, new FuncPredicated(() => {
                var player = PlayerDectector.GetPlayer();
                return Vector3.Distance(transform.position, player.position) <= attackRange;
            }));

            // ★ jump‐attack → chase, once cooldown has elapsed and still out of melee range
            At(jumpAttackState, chaseState, new FuncPredicated(() => {
                var player = PlayerDectector.GetPlayer();
                float dist = Vector3.Distance(transform.position, player.position);
                return dist > attackRange;
                         
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
            Debug.Log("Wolf Attacking");
        }
        public override void Jump(Entity target)
        {
            if (jumpTimer.IsRunning) return;
            jumpTimer.Start();
            Debug.Log("Wolf jumping");
        }
    }
}
