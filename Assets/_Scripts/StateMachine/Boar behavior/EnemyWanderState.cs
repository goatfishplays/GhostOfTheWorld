using UnityEngine;
using UnityEngine.AI;

namespace PlatformerAI
{
    public class EnemyWanderState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly Vector3 startPoint;
        readonly float wanderRadious;

        public EnemyWanderState(BaseEnemy enemy,NavMeshAgent agent, float wanderRadious) : base(enemy)
        {
            this.agent = agent;
            this.startPoint = enemy.transform.position;
            this.wanderRadious = wanderRadious;
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
                var randomDirection = Random.insideUnitSphere * wanderRadious;
                randomDirection += startPoint;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, wanderRadious, 1);
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


