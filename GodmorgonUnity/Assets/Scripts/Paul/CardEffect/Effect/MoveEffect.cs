using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.GameSequencerSpace;

namespace GodMorgon.CardEffect
{
    public class MoveEffect : CardEffect
    {
        /**
        * Apply the move effect by creating the sequence in the gameSequencer
        */
        public override void ApplyEffect(CardEffectData effectData, GameContext context)
        {
            Debug.Log("Move To " + effectData.nbMoves + " Tiles");
            
            //add the move sequence
            GSA_PlayerMove playerMoveAction = new GSA_PlayerMove();
            GameSequencer.Instance.AddAction(playerMoveAction);

            RoomEffectManager.Instance.AddRoomEffectToSequencer(context.nextRoom);
        }
    }
}
