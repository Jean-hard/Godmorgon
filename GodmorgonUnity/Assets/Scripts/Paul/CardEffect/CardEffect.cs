using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.CardEffect
{
    /**
     * Contain all the function for the possible card effect in the game
     */
    public class CardEffect
    {
        //true when the effect will be finished
        protected bool isEffectDone = false;

        public enum EFFECT_TYPE
        {
            DAMAGE

        }

        public void execute()
        {

        }

        public bool EffectDone()
        {
            return true;
        }
    }
}