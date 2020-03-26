﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Timeline;

namespace GodMorgon.StateMachine
{
    /**
     * state where the ringmaster play his action
     */
    public class RingMaster_Turn : State
    {
        public override void OnStartState()
        {
            Debug.Log("On RingMaster turn State");
            TimelineManager.Instance.DoAction();
        }
    }
}