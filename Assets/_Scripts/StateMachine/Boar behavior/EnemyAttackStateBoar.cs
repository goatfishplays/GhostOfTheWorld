using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Utilities;

namespace PlatformerAI
{
    public class EnemyAttackStateBoar : EnemyAttackState
    {
        readonly protected NavMeshAgent agent;
        readonly protected PlayerDectector playerDetector;
        readonly protected BoarSO boarSO;
        readonly protected GameObject attackHitbox;

        private float defaultSpeed;
        private bool isCharging;
        private Vector3 chargeEndDistant;
        private Vector3 chargeDirection;
        private CountdownTimer timer;


        public EnemyAttackStateBoar(
            BaseEnemy enemy,
            NavMeshAgent agent,
            PlayerDectector playerDetector,
            BoarSO boarSO,
            GameObject attackHitbox) 
            : base(enemy)
        {
            this.agent = agent;
            this.playerDetector = playerDetector;
            this.attackHitbox = attackHitbox;
            this.boarSO = boarSO;


            // Call Attack when the hitbox hits a valid entity.
            attackHitbox.GetComponent<Attack>().OnEntityHit += enemy.Attack;

            timer = new CountdownTimer(this.boarSO.attackCooldown);
        }

        public override void OnEnter()
        {
            Debug.Log("Entering attack state");
            defaultSpeed = agent.speed;
            isCharging = false;
            attackHitbox?.SetActive(false);
            if (timer.Progress <= 0)
            {
                timer.Start();
            }
        }

        public override void Update()
        {
            timer.Tick(Time.deltaTime);

            var player = playerDetector.GetPlayer();
            var distance = Vector3.Distance(enemy.transform.position, player.position);

            if (!isCharging && distance <= boarSO.attackRange && !timer.IsRunning)
            {
                //start to charge
                isCharging=true;
                // canDamage=true;
                attackHitbox.SetActive(true);

                chargeDirection = (player.position - enemy.transform.position).normalized;
                chargeEndDistant = enemy.transform.position + chargeDirection * boarSO.chargeDistance;

                //agent setting
                agent.isStopped = false;
                Debug.Log("CHARG");
            }

            if (isCharging)
            {
                //check if we already reach the target
                float remainDistance = Vector3.Distance(enemy.transform.position, chargeEndDistant);

                if (remainDistance <= 0.5f || agent.remainingDistance <= 0.5f)
                {
                    endCharge();
                }
            }
            else
            {
                if (distance <= boarSO.attackRange)
                // Normal chase when not charging
                    agent.isStopped = true;
                agent.SetDestination(player.position);

                var turnTowardNavSteeringTarget = agent.steeringTarget;

                Vector3 direction = (turnTowardNavSteeringTarget - enemy.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * 5);
            }
        }

        void endCharge()
        {
            Debug.Log("No more charging");
            isCharging = false;
            attackHitbox.SetActive(false);

            timer.Start();
            agent.speed = defaultSpeed;
            agent.isStopped = true;
        }

        public override void OnExit()
        {
            attackHitbox.SetActive(false);
            agent.speed = defaultSpeed;
            agent.isStopped = false;
        }
    }
}
