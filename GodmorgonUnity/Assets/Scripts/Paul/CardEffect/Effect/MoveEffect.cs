using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.GameSequencerSpace;

namespace GodMorgon.CardEffect
{
    public class MoveEffect : CardEffect
    {
        /**
        * Apply the move effect by creating the sequence in the gameSequencer
        */
        public override void ApplyEffect(CardEffectData effectData, GameContext context)
        {
            //Trust
            if (effectData.trust)
            {
                if (BuffManager.Instance.IsTrustValidate(effectData.trustNb))
                {
                    PlayerManager.Instance.UpdateMultiplier(2);
                }
            }
            else PlayerManager.Instance.UpdateMultiplier(1);

            //add the move sequence
            GSA_PlayerMove playerMoveAction = new GSA_PlayerMove();
            GameSequencer.Instance.AddAction(playerMoveAction);
        }
    }
}
