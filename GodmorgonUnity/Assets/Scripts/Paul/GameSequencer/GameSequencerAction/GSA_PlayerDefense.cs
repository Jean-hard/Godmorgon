﻿using System.Collections;
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
            //wait the time of the defense particle effect      when exist...
            yield return new WaitForSeconds(1f);
        }
    }
}