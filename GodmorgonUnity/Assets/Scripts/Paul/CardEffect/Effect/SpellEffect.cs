﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.GameSequencerSpace;
using GodMorgon.Player;

namespace GodMorgon.CardEffect
{
    public class SpellEffect : CardEffect
    {
        /**
         * apply the spell on player and create the action in the sequencer
         */
        public override void ApplyEffect(CardEffectData effectData, GameContext context)
        {
            Debug.Log("Spell : ");
            //effect to draw X card
            if (effectData.DrawCard)
            {
                Debug.Log(" - Draw " + effectData.nbCardToDraw + " cards");
                GameManager.Instance.DrawCard(effectData.nbCardToDraw);

                //add the defense sequence
                GSA_Spell playerSpellAction = new GSA_Spell();
                GameSequencer.Instance.AddAction(playerSpellAction);
            }
        }
    }
}