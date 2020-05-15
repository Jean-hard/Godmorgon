using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public class Curse : Action
    {
        public override IEnumerator Execute()
        {
            RoomEffectManager.Instance.CurseRandomRoom();
            Debug.Log("ACTION Curse");
            yield return null;
        }

        public override void Finish()
        {

        }
    }
}
