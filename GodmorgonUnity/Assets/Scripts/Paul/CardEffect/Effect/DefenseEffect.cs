using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.GameSequencerSpace;
using GodMorgon.Player;
using GodMorgon.Timeline;

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
                if (BuffManager.Instance.IsShiverValidate())
                {
                    nbBlock = nbBlock * 2;
                    Debug.Log("Shiver activate");
                }
            }

            //Trust
            if (effectData.trust)
            {
                if (effectData.trustNb == TimelineManager.Instance.nbActualAction)
                {
                    nbBlock = nbBlock * 2;
                    Debug.Log("Trust activate");
                }
            }

            //si l'effet discard les cartes en mains (pour "abnegation")
            if(effectData.isDiscardHand)
            {
                nbBlock = GameEngine.Instance.GetHandCards().Count * nbBlock;
                GSA_DiscardHand discardHandAction = new GSA_DiscardHand();
                GameSequencer.Instance.AddAction(discardHandAction);
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