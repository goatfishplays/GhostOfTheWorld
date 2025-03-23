using UnityEngine;


namespace PlatformerAI
{
    public interface ITransition
    {
        IState To {  get; }

        IPredicated Condition { get; }
    }
}



