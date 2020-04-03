using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.CardEffect
{
    /**
     * Contain all the parameter for any possible effect
     */
    [System.Serializable]
    public class CardEffectData
    {
        public enum CARD_EFFECT_TYPE
        {
            DAMAGE = 0,
            MOVE
        }

        public void execute()
        {

        }
    }
}