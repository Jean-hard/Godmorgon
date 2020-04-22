using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.GameSequencerSpace;

namespace GodMorgon.CardEffect
{
    public class DefenseEffect : CardEffect
    {
        /**
         * Apply defense effect to player and create the defense action in sequencer
         */
        public override void ApplyEffect(CardEffectData effectData, GameContext context)
        {
            //apply effect
            PlayerManager.Instance.AddBlock(effectData.nbBlock);
            Debug.Log("add " + effectData.nbBlock + " to player defense");

            //add the defense sequence
            GSA_PlayerDefense playerDefenseAction = new GSA_PlayerDefense();
            GameSequencer.Instance.AddAction(playerDefenseAction);
        }
    }
}