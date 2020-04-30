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
            PlayerManager.Instance.OnKillerInstinct();

            //wait for 1sec
            yield return new WaitForSeconds(1.0f);
        }
    }
}
