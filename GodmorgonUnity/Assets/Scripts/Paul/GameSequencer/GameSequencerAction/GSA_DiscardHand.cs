using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.CardEffect;

namespace GodMorgon.GameSequencerSpace
{
    public class GSA_DiscardHand : GameSequencerAction
    {
        /**
         * Should apply a visual defense effect
         */
        public override IEnumerator ExecuteAction(GameContext context)
        {
            //launch particle system
            GameManager.Instance.DiscardHand();

            //wait the time of the defense particle effect
            yield return new WaitForSeconds(1f);
        }
    }
}