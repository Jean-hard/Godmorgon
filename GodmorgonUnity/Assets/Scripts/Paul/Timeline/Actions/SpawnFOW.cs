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

            FogMgr.Instance.CoverEntireMap();

            yield return new WaitForSeconds(2f);

            //yield return null;
        }

        public override void Finish()
        {

        }
    }
}
