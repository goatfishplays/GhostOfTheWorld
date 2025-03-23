using UnityEngine;

namespace PlatformerAI
{
    public class Transition : ITransition
    {
        public IState To { get; }

        public IPredicated Condition { get;}

        public Transition(IState to, IPredicated condition)
        {
            To = to;
            Condition = condition;
        }
    }
}

