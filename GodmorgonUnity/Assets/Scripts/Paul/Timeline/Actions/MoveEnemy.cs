﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public class MoveEnemy : Action
    {
        public override void Execute()
        {
            Debug.Log("ACTION Move Enemy");
        }

        public override void Finish()
        {

        }
    }
}