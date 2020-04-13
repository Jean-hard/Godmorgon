using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.CardEffect
{
    public abstract class CardEffect
    {
        public abstract void ApplyEffect(CardEffectData effectData, GameContext context);
    }
}