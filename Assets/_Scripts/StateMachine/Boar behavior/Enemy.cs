//using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Utilities;



namespace PlatformerAI
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDectector))]
    public class Enemy : BaseEnemy
    {
        
        public Entity entity;

        private EntityHealth entityHealth;
        //Animator animator;


        [SerializeField] float wanderRadius = 5f;
        [SerializeField] float attackCooldown = 2f; // cooldown
        [SerializeField] float attackRange = 10f; // unique per enemy
        [SerializeField] float chargeDistance = 20f;
        [SerializeField] float damage = 5f;

        StateMachine StateMachine;
        
        private void Start()
        {
            if (entity == null)
            {
                entity = GetComponent<Entity>();
            }

            attackTimer = new CountdownTimer(attackCooldown);
            StateMachine = new StateMachine();

            var wanderState = new EnemyWanderState(this, agent, wanderRadius);
            var chaseState = new EnemyChaseState(this, agent, PlayerDectector);
            // TODO: chargeSpeed should have a variable or some other solution.
            var attackState = new EnemyAttackStateBoar(this, agent, PlayerDectector, attackRange, chargeDistance, attackRange * 2, attackCooldown);


            // Only allow death state if the enemy has an entity and entity health script.
            if (entity != null && entity.entityHealth != null)
            {
                entityHealth = entity.entityHealth;
                var deathState = new EnemyDeathState(this, agent, entity);
                Any(deathState, new FuncPredicated(() =>
                {
                    return entityHealth.dead;
                }));
            }

            At(wanderState, chaseState, new FuncPredicated(() => PlayerDectector.canDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicated(() => !PlayerDectector.canDetectPlayer()));

            At(chaseState, attackState, new FuncPredicated(() =>
            {
                var player = PlayerDectector.GetPlayer();
                float distance = Vector3.Distance(transform.position, player.position);

                return distance <= attackRange && !attackTimer.IsRunning;
            }));

            At(attackState, chaseState, new FuncPredicated(() =>
            {
                var player = PlayerDectector.GetPlayer();
                float distance = Vector3.Distance(transform.position, player.position);
                return distance > attackRange;
            }));

            StateMachine.SetState(wanderState);

        }

        void At(IState from, IState to, IPredicated condition) => StateMachine.AddTranstion(from, to, condition);
        void Any(IState to, IPredicated condition) => StateMachine.AddAnyTransition(to, condition);

        private void Update()
        {
            StateMachine.Update();
            attackTimer.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        public override void Attack(Entity target)
        {
            if (attackTimer.IsRunning) return;
            attackTimer.Start();
            EntityHealth targetHealth = target.entityHealth;
            if (targetHealth != null)
            {
                Debug.Log("Attacking");
                targetHealth.ChangeHealth(-damage);
            }
        }
        public override void Jump(Entity target)
        {
            throw new System.NotImplementedException();
        }

        

        
    }


}

