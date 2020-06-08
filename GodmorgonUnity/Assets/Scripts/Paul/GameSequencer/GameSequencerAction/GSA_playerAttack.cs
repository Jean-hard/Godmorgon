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


            if(!context.targets.IsDead())
                yield return new WaitForSeconds(context.targets.GetDamageHitDuration());    //wait the time of the hit particle effect
            else
            {
                while (GameManager.Instance.draftPanelActivated)    //Wait the player to choose a card in draft panel
                {
                    yield return null;
                }
            }
        }
    }
}