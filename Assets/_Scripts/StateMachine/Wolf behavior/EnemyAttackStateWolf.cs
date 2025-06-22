using UnityEngine;
using UnityEngine.AI;

namespace PlatformerAI
{
    public class EnemyAttackStateWolf : EnemyAttackState
    {
        readonly NavMeshAgent agent;
        readonly PlayerDectector playerDetector;
        readonly float attackRange;
        readonly GameObject attackHitbox;

        public EnemyAttackStateWolf(BaseEnemy enemy, 
            NavMeshAgent agent, 
            PlayerDectector playerDetector, 
            float attackRange, 
            GameObject attackHitbox = null)
            : base(enemy)
        {
            this.agent = agent;
            this.playerDetector = playerDetector;
            this.attackRange = attackRange;
            this.attackHitbox = attackHitbox;

            // Call Attack when the hitbox hits a valid entity.
            attackHitbox.GetComponent<Attack>().OnEntityHit += enemy.Attack;
        }

        public override void OnEnter()
        {
            Debug.Log("Enter Wolf attack state");
            attackHitbox.SetActive(false);
        }
        public override void Update()
        {
            var player = playerDetector.GetPlayer();
            var distance = Vector3.Distance(player.position,enemy.transform.position);

            if (distance <= attackRange)
            {
                agent.isStopped = true;

                attackHitbox.SetActive(true);

                //Entity playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
                //if (playerEntity != null)
                //{
                    
                //    enemy.Attack(playerEntity); // Damage once
                //}

            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }


        }
        public override void OnExit()
        {
            attackHitbox.SetActive(false);
        }

    }
}
