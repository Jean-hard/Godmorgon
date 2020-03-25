using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public class SpawnFOW : Action
    {
        public override IEnumerator Execute()
        {
            Debug.Log("ACTION Spawn FOW");
            yield return null;
        }

        public override void Finish()
        {

        }
    }
}
