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
            PlayerManager.Instance.MovePlayer();
            while (!PlayerManager.Instance.PlayerMoveDone())
                yield return null;
        }
    }
}