using PlatformerAI;
using UnityEngine.AI;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    readonly NavMeshAgent agent;

    readonly PlayerDectector playerDetector; // Reference to detector, not player

    public EnemyChaseState(BaseEnemy enemy, NavMeshAgent agent, PlayerDectector playerDetector) : base(enemy)
    {
        this.agent = agent;
        this.playerDetector = playerDetector; // Store the detector
    }


    public override void OnEnter()
    {
        Debug.Log("Entered chase state");
    }
    public override void Update()
    {
        agent.isStopped = false;
        // Check if player exists before using
        if (playerDetector.Player != null && playerDetector.canDetectPlayer())
        {
            // This means that if an enemy enters chase state even without detecting the player, they will still path to their location.
            agent.SetDestination(playerDetector.Player.position);

        }
    }
}
