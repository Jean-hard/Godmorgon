using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.GameSequencerSpace;
using GodMorgon.Timeline;

namespace GodMorgon.CardEffect
{
    //enum of all the type of effect in the game
    public enum CARD_EFFECT_TYPE
    {
        DAMAGE = 0,
        MOVE,
        DEFENSE,
        POWER_UP,
        SPELL,
        CURSE
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
        public void PlayCard(BasicCard card, GameContext context)
        {
            //create and apply all the effect of the card
            foreach (CardEffectData effectData in card.effectsData)
            {
                CardEffect effect = CreateEffect(effectData.GetEffectType());
                effect.ApplyEffect(effectData, context);
            }

            //Une fois que les effets ont été appliqués, on lance les actions 
            GameSequencer.Instance.ExecuteActions(context);
            //on met à jour les infos des cartes affichées
            GameManager.Instance.UpdateCardDataDisplay();
            //on indique le nombre de tour que va jouer le RingMaster
            TimelineManager.Instance.SetRingmasterActionRemain(context.card.actionCost);
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
                case CARD_EFFECT_TYPE.DEFENSE: return new DefenseEffect();
                case CARD_EFFECT_TYPE.POWER_UP: return new PowerUpEffect();
                case CARD_EFFECT_TYPE.SPELL: return new SpellEffect();
            }

            return null;
        }


        
    }
}