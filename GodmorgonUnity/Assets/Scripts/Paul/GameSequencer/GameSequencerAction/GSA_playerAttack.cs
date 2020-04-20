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
            //center the hit effect on the target and play it
            GameSequencer.Instance.enemyHitParticle.transform.position = context.targets.GetEntityViewPosition();
            GameSequencer.Instance.enemyHitParticle.Play();

            //update the healthbar of the target
            context.targets.UpdateHealtBar();

            //wait the time of the hit particle effect ---------------------NOT SURE OF THIS---------------------------
            yield return new WaitForSeconds(GameSequencer.Instance.enemyHitParticle.main.duration);
        }
    }
}