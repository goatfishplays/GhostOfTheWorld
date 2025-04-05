using UnityEngine;

namespace PlatformerAI
{
    public abstract class EnemyBaseState : IState
    {
        protected readonly Enemy enemy;
        //for animation
        //protected reaonly Animator animtor

        protected const float crossFadeDuration = 1.0f;

        protected EnemyBaseState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public virtual void OnEnter()
        {
            //no obj
        }
        public virtual void Update()
        {

        }
        public virtual void FixedUpdate()
        {

        }

        public virtual void OnExit()
        {

        }

    }
}

