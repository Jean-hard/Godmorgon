using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public class Defend : Action
    {
        public override IEnumerator Execute()
        {
            Debug.Log("ACTION defend");
            yield return null;
        }

        public override void Finish()
        {

        }
    }
}
