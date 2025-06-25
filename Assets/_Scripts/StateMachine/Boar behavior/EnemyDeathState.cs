using PlatformerAI;
using UnityEngine.AI;
using UnityEngine;
using UnityEditor;

public class EnemyDeathState : EnemyBaseState
{
    readonly NavMeshAgent agent;
    private EntityHealth entityHealth;

    public EnemyDeathState(BaseEnemy enemy, NavMeshAgent agent, EntityHealth entityHealth) : base(enemy)
    {
        this.agent = agent;
        this.entityHealth = entityHealth;
    }

    public override void OnEnter()
    {
        Debug.Log("Dead");
        agent.isStopped = true;

        GameObject.Destroy(entityHealth.gameObject);
    }

}
