using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.CardEffect
{
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

        //public CardEffect CreateEffect(CardEffectData.CARD_EFFECT_TYPE type)
        //{
        //    switch (type)
        //    {
        //        case CARD_EFFECT_TYPE.DAMAGE: return new DamageEffect();
        //        case CARD_EFFECT_TYPE.MOVE: return new MoveEffect();
        //    }

        //    return null;
        //}

        //public void PlayCard(BasicCard card)
        //{
        //    foreach (CardEffectData effectData in card.effectsData)
        //    {
        //        CardEffect effect = CardEffectFactory.CreateEffect(effectData.GetEffectType());
        //        effect.ApplyEffect(gameContext, effectData);
        //    }
        //}

    }
}