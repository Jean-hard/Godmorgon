using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.GameSequencerSpace;

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
            if (context.targets == null)
                Debug.Log("il manque une target dans le contexte !");
            context.targets.TakeDamage(damagePoint);

            //add the attack sequence
            GSA_PlayerAttack playerAttackAction = new GSA_PlayerAttack();
            GameSequencer.Instance.AddAction(playerAttackAction);
        }
    }
}