using UnityEngine;
using UnityEngine.AI;
using Utilities;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PlatformerAI
{
    public class EnemyJumpAttackWolf : EnemyBaseState
    {
        readonly protected NavMeshAgent agent;
        readonly protected PlayerDectector playerDetector;
        readonly protected float jumpTimeLength;
        readonly protected float jumpCooldown;
        readonly protected AnimationCurve heightCurve;

        readonly protected CountdownTimer jumpCooldownTimer;
        protected bool hasJumped;
        protected Coroutine jumpRoutine;

        public EnemyJumpAttackWolf(BaseEnemy enemy,
            NavMeshAgent agent,
            PlayerDectector playerDetector,
            WolfSO wolfSO)
            : base(enemy)
        {
            this.agent = agent;
            this.playerDetector = playerDetector;
            jumpCooldown = wolfSO.jumpCooldown;
            jumpTimeLength = wolfSO.jumpTimeLength;
            heightCurve = wolfSO.heightCurve;

            jumpCooldownTimer = new CountdownTimer(this.jumpCooldown);
        }

        public override void OnEnter()
        {
            hasJumped = false;
            //Debug.Log("Entered Jump Attack State");
        }

        public override void Update()
        {

            jumpCooldownTimer.Tick(Time.deltaTime);
            // only try to jump if cooldown has elapsed and we haven't jumped yet
            if (hasJumped || jumpCooldownTimer.IsRunning)
            {
                //Debug.Log("can't jump! cooldown on!");
                return;
            }

            
            Transform player = playerDetector.GetPlayer();
          
            if (GameObject.FindGameObjectWithTag("Player").TryGetComponent<Entity>(out Entity playerEntity))
            {
                enemy.Jump(playerEntity); 
            }

            // kick off the visible jump
            jumpRoutine ??= enemy.StartCoroutine(JumpTo(player.position));
        }

        protected IEnumerator JumpTo(Vector3 target)
        {
            // Debug.Log("Start JumpTo");

            hasJumped = true;
            agent.isStopped = true;

            Vector3 startPos = enemy.transform.position;
            float heightCurveTime = heightCurve.keys[^1].time;

            float t = 0f;
            // Move Wolf based on t from 0 to jumpTimeLength
            while (t < jumpTimeLength) 
            {
                t += Time.deltaTime;

                float percentageOfJump = t / jumpTimeLength;
                
                // The time t along the height curve adjusted to account for how long the jump is.
                float tHeight = percentageOfJump * heightCurveTime;

                // Move along the horizontal line plus the curve height
                enemy.transform.position =
                    Vector3.Lerp(startPos, target, percentageOfJump) // Horizontal
                    + Vector3.up * heightCurve.Evaluate(tHeight); // Vertical
                yield return null;
            }

            // Debug.Log("Finish JumpTo");

            // unstop the NavMeshAgent so it can resume chasing
            agent.isStopped = false;

            // restart cooldown so we can jump again later
            jumpCooldownTimer.Start();
            jumpRoutine = null;
        }
    }
}


