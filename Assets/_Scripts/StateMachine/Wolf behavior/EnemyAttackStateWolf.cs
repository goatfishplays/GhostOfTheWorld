using UnityEngine;
using UnityEngine.AI;

namespace PlatformerAI
{
    public class EnemyAttackStateWolf : EnemyAttackState
    {
        readonly protected NavMeshAgent agent;
        readonly protected PlayerDectector playerDetector;
        readonly protected WolfSO wolfSO;
        readonly protected GameObject attackHitbox;

        public EnemyAttackStateWolf(BaseEnemy enemy, 
            NavMeshAgent agent, 
            PlayerDectector playerDetector,
            WolfSO wolfSO, 
            GameObject attackHitbox = null)
            : base(enemy)
        {
            this.agent = agent;
            this.playerDetector = playerDetector;
            this.wolfSO = wolfSO;
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

            if (distance <= wolfSO.attackRange)
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
