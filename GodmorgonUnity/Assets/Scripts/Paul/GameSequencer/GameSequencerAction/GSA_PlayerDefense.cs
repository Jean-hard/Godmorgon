using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.CardEffect;

namespace GodMorgon.GameSequencerSpace
{
    public class GSA_PlayerDefense : GameSequencerAction
    {
        /**
         * Should apply a visual defense effect
         */
        public override IEnumerator ExecuteAction(GameContext context)
        {
            //launch particle system
            PlayerManager.Instance.OnShield();

            //wait the time of the defense particle effect
            yield return new WaitForSeconds(PlayerManager.Instance.playerShield.GetDuration());
        }
    }
}