using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.GameSequencerSpace;

namespace GodMorgon.CardEffect
{
    public class MoveEffect : CardEffect
    {
        public override void ApplyEffect(CardEffectData effectData, GameContext context)
        {
            Debug.Log("Move To " + effectData.nbMoves + " Tiles");
            
            GSA_PlayerMove playerMoveAction = new GSA_PlayerMove();
            GameSequencer.Instance.AddAction(playerMoveAction);
        }
    }
}
