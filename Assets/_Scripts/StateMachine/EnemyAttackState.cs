/*using PlatformerAI;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

public abstract class EnemyAttackState : EnemyBaseState
{
    
    protected readonly Transform player;
    protected readonly PlayerDectector playerDetector;
    protected readonly float attackRange;
    protected readonly CountdownTimer attackTimer;
    protected readonly NavMeshAgent agent;

    public CountdownTimer AttackTimer => attackTimer;
    public bool IsInAttackRange => player != null &&
        Vector3.Distance(enemy.transform.position, player.position) <= attackRange;

    protected EnemyAttackState(Enemy enemy, NavMeshAgent agent, PlayerDectector playerDetector,
                             float attackRange, float cooldown) : base(enemy)
    {
        this.agent = agent;
        this.playerDetector = playerDetector;
        this.attackRange = attackRange;
        attackTimer = new CountdownTimer(cooldown);
    }

    public override void Update()
    {
        attackTimer.Tick(Time.deltaTime);

        // Add visual debug for attack range
        Debug.DrawRay(enemy.transform.position, enemy.transform.forward * attackRange, Color.red);
    }
}*/