using PlatformerAI;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathToState : EnemyBaseState
{
    readonly NavMeshAgent agent;
    readonly PlayerDectector playerDetector; // Reference to detector, not player

    public EnemyPathToState(BaseEnemy enemy, NavMeshAgent agent, PlayerDectector playerDetector) : base(enemy)
    {
        this.agent = agent;
        this.playerDetector = playerDetector;
    }


    public override void OnEnter()
    {
        agent.isStopped = false;
        if (playerDetector.Player != null)
        {
            // Paths directly to players state from when they shot you.
            agent.SetDestination(playerDetector.Player.position);
        }
    }
}
