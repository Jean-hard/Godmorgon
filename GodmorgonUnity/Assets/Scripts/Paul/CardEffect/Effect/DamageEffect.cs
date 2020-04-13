using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.CardEffect
{
    /**
     * class for the card damage effect
     */
    public class DamageEffect : CardEffect
    {
        public override void ApplyEffect(CardEffectData effectData, GameContext context)
        {
            int damagePoint = effectData.damagePoint;

            //if player attack an enemy
            context.targets.TakeDamage(damagePoint);
        }
    }
}