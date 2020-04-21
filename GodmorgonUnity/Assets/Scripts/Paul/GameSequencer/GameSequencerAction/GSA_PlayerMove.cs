using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.CardEffect;

/**
 * Action de déplacement du joueur
 */
namespace GodMorgon.GameSequencerSpace
{
    public class GSA_PlayerMove : GameSequencerAction
    {
        public override IEnumerator ExecuteAction(GameContext context)
        {
            PlayerManager.Instance.MovePlayer(context.card.effectsData[0].nbMoves);
            while (!PlayerManager.Instance.PlayerMoveDone())
                yield return null;
        }
    }
}