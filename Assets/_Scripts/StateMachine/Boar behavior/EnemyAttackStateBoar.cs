
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace PlatformerAI
{
    public class EnemyAttackStateBoar : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly PlayerDectector playerDetector;
        readonly float attackRange;
        readonly float chargeSpeed;
        readonly float chargeDistance;
        readonly float chargeCooldown;

        private float defaultSpeed;
        private bool isCharging;
        private Vector3 chargeEndDistant;
        private Vector3 chargeDirection;
        private CountdownTimer timer;
        private bool canDamage; // to prevent multiple hit



        public EnemyAttackStateBoar(
            BaseEnemy enemy,
            NavMeshAgent agent,
            PlayerDectector playerDetector,
            float attackRange = 5f,
            float chargeSpeed = 30f,
            float chargeDistance = 10f,
            float chargeCooldown = 5f) : base(enemy)
        {
            this.agent = agent;
            this.playerDetector = playerDetector;
            this.attackRange = attackRange;
            this.chargeSpeed = chargeSpeed;
            this.chargeDistance = chargeDistance;
            this.chargeCooldown = chargeCooldown;

            timer = new CountdownTimer(chargeCooldown);
        }

        public override void OnEnter()
        {
            Debug.Log("Entering attack state");
            defaultSpeed = agent.speed;
            isCharging = false;
            canDamage = false;
            if (timer.Progress <= 0)
            {
                timer.Start();
            }
        }

        public override void Update()
        {
            timer.Tick(Time.deltaTime);
            Debug.Log(timer.Progress);

            var player = playerDetector.GetPlayer();
            var distance = Vector3.Distance(enemy.transform.position, player.position);

            if (!isCharging && distance <= attackRange && !timer.IsRunning)
            {
                //start to charge
                isCharging=true;
                canDamage=true;

                chargeDirection = (player.position - enemy.transform.position).normalized;
                chargeEndDistant = enemy.transform.position + chargeDirection * chargeDistance;

                //agent setting
                agent.speed = chargeSpeed;
                agent.isStopped = false;
                agent.SetDestination(chargeEndDistant);
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

                if (canDamage && Vector3.Distance(enemy.transform.position, player.position) < 1.5f)
                {
                    Entity playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
                    if (playerEntity != null)
                    {
                        enemy.Attack(playerEntity); // Damage once
                    }
                    canDamage = false; // Prevent multiple hits
                    Debug.Log("Boar HIT the player!");
                }

            }
            else
            {
                if (distance <= attackRange)
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
            canDamage = false;
            timer.Start();
            agent.speed = defaultSpeed;
            agent.isStopped = true;
        }

        public override void OnExit()
        {
            agent.speed = defaultSpeed;
            agent.isStopped = false;
        }
    }
}
