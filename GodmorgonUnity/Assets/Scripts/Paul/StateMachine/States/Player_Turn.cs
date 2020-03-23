using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.StateMachine
{
    public class Player_Turn : State
    {
        public override void OnStartState()
        {
            Debug.Log("On Player turn State");
        }
    }
}