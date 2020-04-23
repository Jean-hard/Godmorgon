using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.CardEffect;

namespace GodMorgon.GameSequencerSpace
{
    public class GSA_PowerUp : GameSequencerAction
    {
        /**
         * Should apply a visual power up effect
         */
        public override IEnumerator ExecuteAction(GameContext context)
        {
            //launch particle system
            PlayerManager.Instance.OnPowerUp();

            //wait the time of the power up particle effect
            yield return new WaitForSeconds(PlayerManager.Instance.playerPowerUp.GetDuration());
        }
    }
}