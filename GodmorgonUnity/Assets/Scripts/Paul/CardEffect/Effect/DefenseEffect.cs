using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.GameSequencerSpace;
using GodMorgon.Player;

namespace GodMorgon.CardEffect
{
    public class DefenseEffect : CardEffect
    {
        /**
         * Apply defense effect to player and create the defense action in sequencer
         */
        public override void ApplyEffect(CardEffectData effectData, GameContext context)
        {
            int nbBlock = effectData.nbBlock;

            //Shiver
            if (effectData.shiver)
            {
                if (PlayerData.Instance.IsHealthAtHalf())
                {
                    nbBlock = nbBlock * 2;
                    Debug.Log("Shiver activate");
                }
            }

            //apply effect
            PlayerManager.Instance.AddBlock(nbBlock);
            Debug.Log("add " + nbBlock + " to player defense");

            //add the defense sequence
            GSA_PlayerDefense playerDefenseAction = new GSA_PlayerDefense();
            GameSequencer.Instance.AddAction(playerDefenseAction);
        }
    }
}