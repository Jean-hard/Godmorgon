using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.CardEffect;

namespace GodMorgon.GameSequencerSpace
{
    public class GSA_PlayerCounter : GameSequencerAction
    {
        /**
         * Should apply a visual counter activation effect
         */
        public override IEnumerator ExecuteAction(GameContext context)
        {
            //launch particle system
            //......

            //wait the time of the defense particle effect
            yield return new WaitForSeconds(0.5f);
        }
    }
}
