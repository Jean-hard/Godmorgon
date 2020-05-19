using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.CardEffect;

namespace GodMorgon.GameSequencerSpace
{
    public class GSA_FastShoes : GameSequencerAction
    {
        /**
         * Should apply a visual effect the Fast shoes activation
         */
        public override IEnumerator ExecuteAction(GameContext context)
        {
            //launch particle system
            //......

            //wait the time of the fast shoes particle effect
            yield return new WaitForSeconds(0.5f);
        }
    }
}
