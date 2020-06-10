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
                if (BuffManager.Instance.IsTrustValidate(effectData.trustNb))
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

            //si l'effet active le counter
            if(effectData.isCounter)
            {
                BuffManager.Instance.isCounterActive = true;
                BuffManager.Instance.counterDamage = effectData.counterDamagePoint;
                GSA_PlayerCounter counterAction = new GSA_PlayerCounter();
                GameSequencer.Instance.AddAction(counterAction);

                PlayerManager.Instance.OnPlayerCounter();
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