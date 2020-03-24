using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.DeckBuilding;

namespace GodMorgon.StateMachine
{
    public class Draft : State
    {
        public override void OnStartState()
        {
            Debug.Log("On Draft State");

            DeckBuildingManager.Instance.DraftStart();
        }
    }
}