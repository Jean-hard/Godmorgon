using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public class Curse : Action
    {
        public override IEnumerator Execute()
        {
            RoomEffectManager.Instance.CurseSpecificRoom(); //Curse une room spécifiée dans l'inspector
            //RoomEffectManager.Instance.CurseRandomRoom(); //Curse une room random autour du player (comportement normal)
            Debug.Log("ACTION Curse");

            yield return new WaitForSeconds(2f);

            //yield return null;
        }

        public override void Finish()
        {

        }
    }
}
