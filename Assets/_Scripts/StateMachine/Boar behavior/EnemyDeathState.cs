using PlatformerAI;
using UnityEngine.AI;
using UnityEngine;
using UnityEditor;

public class EnemyDeathState : EnemyBaseState
{
    readonly NavMeshAgent agent;
    private Entity entity;

    public EnemyDeathState(Enemy enemy, NavMeshAgent agent, Entity entity) : base(enemy)
    {
        this.agent = agent;
        this.entity = entity;
    }

    public override void OnEnter()
    {
        // Debug.Log("Dead");
        agent.isStopped = true;
        GameObject.Destroy(entity.gameObject);
    }
}
