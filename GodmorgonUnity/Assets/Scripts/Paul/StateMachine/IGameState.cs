using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.StateMachine
{
    public interface IGameState
    {
        void OnStartState();

        void OnEndState();
    }
}