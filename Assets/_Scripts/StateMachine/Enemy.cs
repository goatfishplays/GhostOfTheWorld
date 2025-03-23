using UnityEngine;
using UnityEngine.AI;



namespace PlatformerAI
{
    [RequireComponent (typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        public NavMeshAgent agent;

        //Animator animator;

        StateMachine StateMachine;

        private void Start()
        {
            StateMachine = new StateMachine ();

            var wanderState = new EnemyWanderState(this, agent, 5f);

            Any(wanderState, new FuncPredicated(()=> true)); //Always true

            StateMachine.SetState (wanderState);


        }

        void At(IState from, IState to, IPredicated condition) => StateMachine.AddTranstion(from, to, condition);
        void Any(IState to, IPredicated condition) => StateMachine.AddAnyTransition(to, condition);

        private void Update()
        {
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();    
        }
    }
    
    
}

