using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.GameSequencerSpace;
using GodMorgon.Player;
using GodMorgon.Timeline;

namespace GodMorgon.CardEffect
{
    public class SpellEffect : CardEffect
    {
        bool isTrustActivate = false;

        /**
         * apply the spell on player and create the action in the sequencer
         */
        public override void ApplyEffect(CardEffectData effectData, GameContext context)
        {
            //Trust
            if (effectData.trust)
            {
                if (effectData.trustNb == TimelineManager.Instance.nbActualAction)
                {
                    Debug.Log("Trust activate");
                    isTrustActivate = true;
                }
            }

            Debug.Log("Spell : ");
            //effect to draw X card
            if (effectData.DrawCard)
            {
                int nbCardToDraw = effectData.nbCardToDraw;
                if (isTrustActivate)
                    nbCardToDraw = nbCardToDraw * 2;

                GameManager.Instance.DrawCard(nbCardToDraw);
                Debug.Log(" - Draw " + nbCardToDraw + " cards");

                //add the Spell sequence
                GSA_Spell playerSpellAction = new GSA_Spell();
                GameSequencer.Instance.AddAction(playerSpellAction);
            }

            //add the sight sequence
            if(effectData.Sight)
            {
                FogMgr.Instance.SetRevealRange(effectData.sightRange);
                GSA_Sight sightAction = new GSA_Sight();
                GameSequencer.Instance.AddAction(sightAction);
            }
        }
    }
}