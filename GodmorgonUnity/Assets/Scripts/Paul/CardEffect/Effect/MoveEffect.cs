using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.CardEffect
{
    public class MoveEffect : CardEffect
    {
        public override void ApplyEffect(CardEffectData effectData)
        {
            Debug.Log("Move To " + effectData.movePoint + " Tiles");
            //PlayerManager.Instance.MovePlayer();
        }
    }
}