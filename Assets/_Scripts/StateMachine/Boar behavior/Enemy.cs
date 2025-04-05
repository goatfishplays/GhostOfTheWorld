//using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Utilities;



namespace PlatformerAI
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDectector))]
    public class Enemy : MonoBehaviour
    {
        public NavMeshAgent agent;
        public PlayerDectector PlayerDectector;
        //Animator animator;

        [SerializeField] float wanderRadious = 5f;
        [SerializeField] float attackCooldown = 2f; // cooldown
        [SerializeField] float attackRange = 10f; // unique per enemy

        StateMachine StateMachine;
        CountdownTimer attackTimer;
        private void Start()
        {
            attackTimer = new CountdownTimer(attackCooldown);
            StateMachine = new StateMachine();

            var wanderState = new EnemyWanderState(this, agent, wanderRadious);
            var chaseState = new EnemyChaseState(this, agent, PlayerDectector);
            var attackState = new EnemyAttackStateBoar(this, agent, PlayerDectector, attackRange);


            At(wanderState, chaseState, new FuncPredicated(() => PlayerDectector.canDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicated(() => !PlayerDectector.canDetectPlayer()));

            At(chaseState, attackState, new FuncPredicated(() =>
            {
                var player = PlayerDectector.GetPlayer();
                float distance = Vector3.Distance(transform.position, player.position);
                return distance <= attackRange;
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

        public void attack()
        {
            
            if (attackTimer.IsRunning) return;
            attackTimer.Start();
            Debug.Log("Attacking");
        }


    }


}

