using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.GameSequencerSpace;
using GodMorgon.Player;

namespace GodMorgon.CardEffect
{
    public class PowerUpEffect : CardEffect
    {
        /**
         * apply the effect on player and create the action in the sequencer
         */
        public override void ApplyEffect(CardEffectData effectData, GameContext context)
        {
            Debug.Log("add Power Up : ");
            if(effectData.KillerInstinct)
            {
                Debug.Log(" - Killer Instinct !");
                PlayerData.Instance.OnKillerInstinct();
            }
        }
    }
}