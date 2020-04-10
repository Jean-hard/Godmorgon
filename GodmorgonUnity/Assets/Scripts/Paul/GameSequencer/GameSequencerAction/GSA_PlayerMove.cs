using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Action de déplacement du joueur
 */
public class GSA_PlayerMove : GameSequencerAction
{
    public override IEnumerator ExecuteAction()
    {
        PlayerManager.Instance.MovePlayer();
        while (!PlayerManager.Instance.PlayerMoveDone())
            yield return null;
    }
}
