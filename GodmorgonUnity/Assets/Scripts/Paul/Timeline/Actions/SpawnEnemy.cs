using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public class SpawnEnemy : Action
    {
        public override void Execute()
        {
            Debug.Log("ACTION spawn enemy");
        }

        public override void Finish()
        {

        }
    }
}
