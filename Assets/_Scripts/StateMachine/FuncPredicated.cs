using UnityEngine;
using System;

namespace PlatformerAI
{
    public class FuncPredicated : IPredicated
    {
        readonly Func<bool> func;

        public FuncPredicated(Func<bool> func)
        {
            this.func = func;
        }

        public bool Evaluate() => func.Invoke();
    }
}
