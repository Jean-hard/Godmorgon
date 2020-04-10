using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.GameSequencerSpace;

namespace GodMorgon.CardEffect
{
    //enum of all the type of effect in the game
    public enum CARD_EFFECT_TYPE
    {
        DAMAGE = 0,
        MOVE
    }

    /**
     * Manage the effect when a card is played
     */
    public class CardEffectManager
    {

        #region Singleton Pattern
        private static CardEffectManager instance;

        public static CardEffectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CardEffectManager();
                }

                return instance;
            }
        }
        #endregion

        /**
         * Play the effect of the card
         */
        public void PlayCard(BasicCard card)
        {
            //create and apply all the effect of the card
            foreach (CardEffectData effectData in card.effectsData)
            {
                CardEffect effect = CreateEffect(effectData.GetEffectType());
                effect.ApplyEffect(effectData);
            }

            //Une fois que les effets ont été appliqués, on lance les actions
            GameSequencer.Instance.ExecuteActions();
        }


        /**
         * Factory of card effect
         * create a cardEffect object depending to the effect type in parameter
         */
        public CardEffect CreateEffect(CARD_EFFECT_TYPE type)
        {
            switch (type)
            {
                case CARD_EFFECT_TYPE.DAMAGE: return new DamageEffect();
                case CARD_EFFECT_TYPE.MOVE: return new MoveEffect();
            }

            return null;
        }


        
    }
}