using UnityEngine;

namespace PlatformerAI
{
    public interface IState
    {
        void OnEnter();
        void Update();
        void FixedUpdate();

        void OnExit();

    }

    /*public abstract class BaseState : IState
    {

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

    }*/

    //IpredicatedStae
    



}





