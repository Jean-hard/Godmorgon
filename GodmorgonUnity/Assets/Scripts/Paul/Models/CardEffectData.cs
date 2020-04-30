﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.CardEffect;

namespace GodMorgon.Models
{
    /**
     * Contain all the parameter for any possible effect
     */
    [System.Serializable]
    public class CardEffectData
    {
        //current effect select
        public CARD_EFFECT_TYPE currentEffect = CARD_EFFECT_TYPE.DAMAGE;

        public CARD_EFFECT_TYPE GetEffectType()
        {
            return currentEffect;
        }



        /**
         * All the parameter for all the possible effect
         * The inspector must adapte to show only the parameter needed,
         * depending on the type of the effect.
         */

        [Header("Damage")]
        //nb damage deal
        public int damagePoint = 0;


        [Header("Movement")]
        //nb movement
        public int nbMoves = 0;


        [Header("Defense")]
        //nb block
        public int nbBlock = 0;


        [Header("Power Up")]
        //double damage done and take
        public bool KillerInstinct = false;


        [Header("Spell")]
        //effect to draw card
        public bool DrawCard = false;
        public int nbCardToDraw = 0;

        [Header("Other")]
        //Shiver : double les valeurs si la vie est inférieurs à 50%
        public bool shiver = false;
        //Trust : active des bonus par rapport au tour de la timeline du ringmaster
        public bool trust = false;
        public int trustNb = 0;
    }
}