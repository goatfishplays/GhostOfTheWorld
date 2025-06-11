using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace PlatformerAI
{
    public class EnemyAttackStateSpirit : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly PlayerDectector playerDetector;
        readonly float attackRange;
        readonly float shootCooldown;


        private CountdownTimer timer;
        //private bool canDamage; // to prevent multiple hit



        public EnemyAttackStateSpirit(
            BaseEnemy enemy,
            NavMeshAgent agent,
            PlayerDectector playerDetector,
            float attackRange = 5f,
            float shootCooldown = 30f
            ) : base(enemy)
        {
            this.agent = agent;
            this.playerDetector = playerDetector;
            this.attackRange = attackRange;
            this.shootCooldown = shootCooldown;


            timer = new CountdownTimer(shootCooldown);
        }

        public override void OnEnter()
        {
            Debug.Log("Entering attack state");
            //canDamage = false;
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
            RaycastHit hit;
            Vector3 directionToPlayer = (player.position - enemy.transform.position).normalized;

            if (distance < attackRange && !timer.IsRunning &&
                Physics.Raycast(enemy.transform.position, directionToPlayer, out hit, attackRange))
            {
                agent.isStopped = false;
                if (hit.collider.CompareTag("Player"))
                {
                    
                    Entity playerEntity = hit.collider.GetComponent<Entity>();
                    if (playerEntity != null)
                    {
                        enemy.Attack(playerEntity); // Damage once
                        timer.Start();
                    }
                }
            }

            else
            {
                agent.isStopped = true;
                agent.SetDestination(player.position);
                var turnTowardNavSteeringTarget = agent.steeringTarget;

                Vector3 direction = (turnTowardNavSteeringTarget - enemy.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * 5);
            }

        }

    }
}


