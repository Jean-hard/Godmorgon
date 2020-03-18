using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.StateMachine
{
    public abstract class State : IGameState
    {
        public virtual void OnStartState() { }

        public void OnEndState() { }
    }
}