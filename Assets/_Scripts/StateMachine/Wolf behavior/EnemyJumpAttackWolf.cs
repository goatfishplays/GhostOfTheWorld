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
        readonly NavMeshAgent agent;
        readonly PlayerDectector playerDetector;
        readonly float jumpRangeMax;
        readonly float jumpRangeMin;
        readonly float jumpTimeLength;
        readonly float jumpCooldown;
        readonly AnimationCurve heightCurve;

        private CountdownTimer jumpCooldownTimer;
        private bool hasJumped;
        private Coroutine jumpRoutine;

        public EnemyJumpAttackWolf(BaseEnemy enemy,
            NavMeshAgent agent,
            PlayerDectector playerDetector,
            AnimationCurve heightCurve,
            float jumpRangeMax = 10f,
            float jumpRangeMin = 15,
            float jumpTimeLength = 2f,       // this now controls how fast the arc plays out
            float jumpCooldown = 1f) 
            : base(enemy)
        {
            this.agent = agent;
            this.jumpRangeMax = jumpRangeMax;
            this.jumpRangeMin = jumpRangeMin;
            this.playerDetector = playerDetector;
            this.jumpCooldown = jumpCooldown;
            this.jumpTimeLength = jumpTimeLength;
            this.heightCurve = heightCurve;

            jumpCooldownTimer = new CountdownTimer(this.jumpCooldown);
        }

        public override void OnEnter()
        {
            hasJumped = false;
            Debug.Log("Entered Jump Attack State");
        }

        public override void Update()
        {

            jumpCooldownTimer.Tick(Time.deltaTime);
            // only try to jump if cooldown has elapsed and we haven't jumped yet
            if (hasJumped || jumpCooldownTimer.IsRunning)
            {
                Debug.Log("can't jump! cooldown on!");
                return;
            }

            
            Transform player = playerDetector.GetPlayer();
          
            Entity playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
            if (playerEntity != null)
            {
                enemy.Jump(playerEntity); 
            }

            // kick off the visible jump
            if (jumpRoutine == null)
                jumpRoutine = enemy.StartCoroutine(JumpTo(player.position));

        }

        private IEnumerator JumpTo(Vector3 target)
        {
            Debug.Log("Start JumpTo");

            hasJumped = true;
            agent.isStopped = true;

            Vector3 startPos = enemy.transform.position;
            float t = 0f;

            // Get the end time of the height curve. Last element is always the latest of time.
            float endTime = heightCurve.keys[heightCurve.keys.Length - 1].time;

            // Move Wolf based on t from 0 to endTime
            while (t < endTime) 
            {
                t += Time.deltaTime;
                // float clamped = Mathf.Clamp01(t);
                float percentageOfTotalTime = t / endTime;
                // move along the horizontal line plus the curve height
                enemy.transform.position =
                    Vector3.Lerp(startPos, target, percentageOfTotalTime) // Horizontal
                    + Vector3.up * heightCurve.Evaluate(t); // Vertical
                yield return null;
            }

            Debug.Log("Finish JumpTo");

            // land exactly at the target
            enemy.transform.position = target;

            // unstop the NavMeshAgent so it can resume chasing
            agent.isStopped = false;

            // restart cooldown so we can jump again later
            jumpCooldownTimer.Start();
            jumpRoutine = null;
        }
    }
}


