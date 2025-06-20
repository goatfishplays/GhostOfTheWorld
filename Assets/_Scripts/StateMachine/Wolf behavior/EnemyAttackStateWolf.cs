using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace PlatformerAI
{
    public class EnemyAttackStateWolf : EnemyAttackState
    {
        readonly NavMeshAgent agent;
        readonly PlayerDectector playerDetector;
        readonly float attackRange;

        public EnemyAttackStateWolf(BaseEnemy enemy, NavMeshAgent agent, PlayerDectector playerDetector, float attackRange): base(enemy)
        {
            this.agent = agent;
            this.playerDetector = playerDetector;
            this.attackRange = attackRange;
        }

        public override void OnEnter()
        {
            Debug.Log("Wolf attack");
        }
        public override void Update()
        {
            var player = playerDetector.GetPlayer();
            var distance = Vector3.Distance(player.position,enemy.transform.position);

            if (distance <= attackRange)
            {
                agent.isStopped = true;
                Entity playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
                if (playerEntity != null)
                {
                    enemy.Attack(playerEntity); // Damage once
                }

            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }


        }

    }
}
