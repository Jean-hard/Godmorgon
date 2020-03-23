using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public class Attack : Action
    {
        public override void Execute()
        {
            Debug.Log("ACTION attack");
        }

        public override void Finish()
        {

        }
    }
}
