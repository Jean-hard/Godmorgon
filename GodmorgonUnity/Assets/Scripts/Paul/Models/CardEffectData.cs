using System.Collections;
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

        //nb damage deal
        public int damagePoint = 0;

        //nb movement
        public int nbMoves = 0;

    }
}