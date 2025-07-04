using UnityEngine;
using UnityEngine.AI;

namespace PlatformerAI
{
    public class EnemyWanderState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        
        readonly float wanderRadius;
        

        public EnemyWanderState(BaseEnemy enemy,NavMeshAgent agent, float wanderRadius) : base(enemy)
        {
            this.agent = agent;
            this.wanderRadius = wanderRadius;
        }

        public override void OnEnter()
        {
            Debug.Log("Wander");
        }

        public override void Update()
        {
            agent.isStopped = false;
           if (HasReachedDestination())
            {
                //find a new destination
                var randomDirection = Random.insideUnitSphere * wanderRadius;
                randomDirection += enemy.transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
                var finalPosition = hit.position;

                agent.SetDestination(finalPosition);
            }

        }

        bool HasReachedDestination()
        {
            return !agent.pathPending &&
                agent.remainingDistance <= agent.stoppingDistance &&
                (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
        }

    }


}


