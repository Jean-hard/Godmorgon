using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public class Attack : Action
    {
        public override IEnumerator Execute()
        {
            Debug.Log("ACTION attack");
            yield return null;
        }

        public override void Finish()
        {

        }
    }
}
