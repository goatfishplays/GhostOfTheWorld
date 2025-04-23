using UnityEngine;
using UnityEngine.AI;
using Utilities;
using System.Collections;

namespace PlatformerAI
{
    public class EnemyJumpAttackWolf : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly PlayerDectector playerDetector;
        readonly float jumpRange;
        readonly float jumpSpeed;
        readonly float jumpCooldown;
        readonly AnimationCurve heightCurve;

        private CountdownTimer jumpCooldownTimer;
        private bool hasJumped;
        private Coroutine jumpRoutine;

        public EnemyJumpAttackWolf(BaseEnemy enemy,
            NavMeshAgent agent,
            PlayerDectector playerDetector,
            AnimationCurve heightCurve,
            float jumpRange = 10f,
            float jumpSpeed = 2f,       // this now controls how fast the arc plays out
            float jumpCooldown = 1f) : base(enemy)
        {
            this.agent = agent;
            this.jumpRange = jumpRange;
            this.playerDetector = playerDetector;
            this.jumpCooldown = jumpCooldown;
            this.jumpSpeed = jumpSpeed;
            this.heightCurve = heightCurve;

            jumpCooldownTimer = new CountdownTimer(jumpCooldown);
        }

        public override void OnEnter()
        {
            hasJumped = false;
           
        }

        public override void Update()
        {

            jumpCooldownTimer.Tick(Time.deltaTime);
            // only try to jump if cooldown has elapsed and we haven't jumped yet
            if (hasJumped || jumpCooldownTimer.IsRunning)
                return;
            Debug.Log("Jump");
            Transform player = playerDetector.GetPlayer();
            float distance = Vector3.Distance(enemy.transform.position, player.position);
            if (distance <= jumpRange)
            {
                Entity playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
                if (playerEntity != null)
                {
                    enemy.Jump(playerEntity); // Damage once
                }
                
                // kick off the visible jump
                if (jumpRoutine == null)
                    jumpRoutine = enemy.StartCoroutine(JumpTo(player.position));
            }
        }

        private IEnumerator JumpTo(Vector3 target)
        {
            hasJumped = true;
            agent.isStopped = true;

            Vector3 startPos = enemy.transform.position;
            float t = 0f;

            // animate t from 0?1 over time
            while (t < 1f)
            {
                t += Time.deltaTime * jumpSpeed;
                float clamped = Mathf.Clamp01(t);
                // move along the horizontal line plus the curve height
                enemy.transform.position =
                    Vector3.Lerp(startPos, target, clamped)
                    + Vector3.up * heightCurve.Evaluate(clamped);
                yield return null;
            }

            // land exactly at the target
            enemy.transform.position = target;

            // un?stop the NavMeshAgent so it can resume chasing
            agent.isStopped = false;

            // restart cooldown so we can jump again later
            jumpCooldownTimer.Start();
            jumpRoutine = null;
        }
    }
}


