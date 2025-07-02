using PlatformerAI;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace PlatformerAI
{
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(EntityHealth))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDectector))]

    // This is an enemy that has an attack and wanders.
    public abstract class AttackEnemy : BaseEnemy
    {
        [Tooltip("The entity that this script is attached to.")]
        [SerializeField] protected EntityHealth entityHealth;
        //Animator animator;

        [Tooltip("Max distance they will randomly wander to from their current location. Basically how far they move each movement when wandering")]
        [SerializeField] public float wanderRadius = 5f;
        [Tooltip("Time between attacks")]
        [SerializeField] public float attackCooldown = 2f;
        [Tooltip("Distance before the enemy starts doing attacks.")]
        [SerializeField] public float attackRange = 10f;



        protected StateMachine StateMachine = null;
        protected EnemyWanderState wanderState = null;
        protected EnemyChaseState chaseState = null;
        protected EnemyAttackState attackState = null;
        protected EnemyPathToState pathToState = null;

        // A lambda function that defines a transition from a specified state to another
        protected void At(IState from, IState to, IPredicated condition) => StateMachine.AddTranstion(from, to, condition);
        // From any state on condition, used for things that may activate at anytime like enemy dying.
        protected void Any(IState to, IPredicated condition) => StateMachine.AddAnyTransition(to, condition);


        protected virtual void Start()
        {
            // Check if entityHealth is null
            if (entityHealth == null)
            {
                entityHealth = GetComponent<EntityHealth>();
            }
            
            StateMachine = new StateMachine();

            // ----- State definitions -----
            wanderState = new EnemyWanderState(this, agent, wanderRadius);
            chaseState = new EnemyChaseState(this, agent, PlayerDectector);
            pathToState = new EnemyPathToState(this, agent, PlayerDectector);

            // Note: Attack state is defined by the derived class.
            if (attackState == null)
            {
                throw new NullReferenceException("attackState is not defined.");
            }
            
            // Only allow death state if the enemy has an entity health script.
            // Note: this might be more optimized if we don't use the state machine and make it use events instead of checking every update.
            if (entityHealth != null)
            {
                var deathState = new EnemyDeathState(this, agent, entityHealth);
                Any(deathState, new FuncPredicated(() =>
                {
                    return entityHealth.dead;
                }));
            }

            // ----- State Transitions -----
            // Wander -> Chase: When player is detected
            At(wanderState, chaseState, new FuncPredicated(() => PlayerDectector.canDetectPlayer()));
            // Chase -> Wander: When player can't be detected.
            At(chaseState, wanderState, new FuncPredicated(() => !PlayerDectector.canDetectPlayer()));

            At(pathToState, chaseState, new FuncPredicated(() => PlayerDectector.canDetectPlayer()));

            At(pathToState, wanderState, new FuncPredicated(() =>
            {
                // Check not processing
                if (!agent.pathPending)
                {
                    // Check within range
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        // Check either at position or not moving
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }));

            // Chase -> Attack: when player is within attackRange.
            At(chaseState, attackState, new FuncPredicated(() =>
            {
                var player = PlayerDectector.GetPlayer();
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                return distanceToPlayer <= attackRange;
            }));

            // Attack -> Chase: When player is outside of attack range
            At(attackState, chaseState, new FuncPredicated(() =>
            {
                var player = PlayerDectector.GetPlayer();
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                return distanceToPlayer > attackRange;
            }));

            // Try to path to player position when hit.
            // Possible bug: Might try if there's something that hits them but it's not the player.
            entityHealth.OnHit += enemyHit;

            // Set starting state to wandering
            StateMachine.SetState(wanderState);
        }

        protected virtual void Update()
        {
            StateMachine.Update();
        }

        // This currently doesn't do anything as StateMachine doesn't have implementation for FixedUpdate()
        private void FixedUpdate()
        {
            // StateMachine.FixedUpdate();
        }
        
        protected void enemyHit(float delta)
        {
            StateMachine.TryChangeState(wanderState, pathToState);
        }
    }
}
