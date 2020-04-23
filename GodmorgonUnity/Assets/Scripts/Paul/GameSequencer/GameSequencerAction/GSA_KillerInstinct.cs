using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.CardEffect;

namespace GodMorgon.GameSequencerSpace
{
    public class GSA_KillerInstinct : GameSequencerAction
    {
        public override IEnumerator ExecuteAction(GameContext context)
        {
            //wait the time of the killerInstinct particle effect      when exist...
            yield return new WaitForSeconds(1f);
        }
    }
}
