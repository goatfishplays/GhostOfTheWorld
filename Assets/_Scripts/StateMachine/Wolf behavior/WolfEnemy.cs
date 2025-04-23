using UnityEngine;
using UnityEngine.AI;
using Utilities;



namespace PlatformerAI
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDectector))]
    public class WolfEnemy : BaseEnemy
    {
        
        //Animator animator;

        [SerializeField] float wanderRadious = 5f;
        [SerializeField] float attackCooldown = 2f; // cooldown
        [SerializeField] float attackRange = 2f; // unique per enemy
        [SerializeField] float jumpRange = 10f;
        [SerializeField] float jumpCooldown = 5f;
        [SerializeField] AnimationCurve HeightCurve;

        [SerializeField] CountdownTimer jumpTimer;
        

        StateMachine StateMachine;

        private void Start()
        {
            attackTimer = new CountdownTimer(attackCooldown);
            jumpTimer = new CountdownTimer(jumpCooldown);
            StateMachine = new StateMachine();

            var wanderState = new EnemyWanderState(this, agent, wanderRadious);
            var chaseState = new EnemyChaseState(this, agent, PlayerDectector);
            var attackState = new EnemyAttackStateWolf(this, agent, PlayerDectector, attackRange);

            // pass jumpSpeed _then_ jumpCooldown
            var jumpAttackState = new EnemyJumpAttackWolf(
                this,
                agent,
                PlayerDectector,
                HeightCurve,
                jumpRange
                
            );

            // wander ↔ chase
            At(wanderState, chaseState, new FuncPredicated(() => PlayerDectector.canDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicated(() => !PlayerDectector.canDetectPlayer()));


            // chase -> attack
            At(chaseState, attackState, new FuncPredicated(() => {
                var player = PlayerDectector.GetPlayer();
                float dist = Vector3.Distance(transform.position, player.position);
                return dist <= attackRange;

            }));

            At(attackState, jumpAttackState, new FuncPredicated(() => {
                var player = PlayerDectector.GetPlayer();
                float dist = Vector3.Distance(transform.position, player.position);
                return dist > attackRange && dist <= jumpRange;

            }));

            // chase → jump‐attack
            At(chaseState, jumpAttackState, new FuncPredicated(() => {
                var player = PlayerDectector.GetPlayer();
                float dist = Vector3.Distance(transform.position, player.position);
                return dist <= jumpRange
                    && dist > attackRange
                    ;       // only if our cooldown is ready
            }));

            // jump‐attack → melee‐attack
            At(jumpAttackState, attackState, new FuncPredicated(() => {
                var player = PlayerDectector.GetPlayer();
                return Vector3.Distance(transform.position, player.position) <= attackRange;
            }));

            // ★ jump‐attack → chase, once cooldown has elapsed and still out of melee range
            At(jumpAttackState, chaseState, new FuncPredicated(() => {
                var player = PlayerDectector.GetPlayer();
                float dist = Vector3.Distance(transform.position, player.position);
                return dist > attackRange;
                         
            }));

            StateMachine.SetState(wanderState);
        }

        void At(IState from, IState to, IPredicated condition) => StateMachine.AddTranstion(from, to, condition);
        void Any(IState to, IPredicated condition) => StateMachine.AddAnyTransition(to, condition);

        private void Update()
        {
            StateMachine.Update();
            attackTimer.Tick(Time.deltaTime);
            jumpTimer.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        public override void Attack(Entity target)
        {

            if (attackTimer.IsRunning) return;
            attackTimer.Start();
            Debug.Log("Wolf Attacking");
        }
        public override void Jump(Entity target)
        {
            if (jumpTimer.IsRunning) return;
            jumpTimer.Start();
            Debug.Log("Wolf jumping");
        }

    }


}
