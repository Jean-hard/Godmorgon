using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public class SpawnEnemy : Action
    {
        public override IEnumerator Execute()
        {
            Debug.Log("ACTION spawn enemy");
            yield return null;
        }

        public override void Finish()
        {

        }
    }
}
