using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.CardEffect;

namespace GodMorgon.GameSequencerSpace
{
    public class GSA_PlayerAttack : GameSequencerAction
    {
        public override IEnumerator ExecuteAction(GameContext context)
        {
            //show damage effect
            context.targets.OnDamage();

            //update the healthbar of the target
            context.targets.UpdateHealthBar();

            //wait the time of the hit particle effect
            yield return new WaitForSeconds(context.targets.GetDamageHitDuration());
        }
    }
}