using UnityEngine;

namespace PlatformerAI
{
    public abstract class EnemyBaseState : IState
    {
        protected readonly BaseEnemy enemy;

        public EnemyBaseState(BaseEnemy enemy)
        {
            this.enemy = enemy;
        }

        protected const float crossFadeDuration = 1.0f;

        

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

