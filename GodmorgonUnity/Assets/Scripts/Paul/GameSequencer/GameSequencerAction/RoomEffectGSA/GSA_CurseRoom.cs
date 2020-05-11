using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodMorgon.CardEffect;

/**
 * Effet Rest d'une room
 */
namespace GodMorgon.GameSequencerSpace
{
    public class GSA_CurseRoom : GameSequencerAction
    {
        public override IEnumerator ExecuteAction(GameContext context)
        {
            RoomEffectManager.Instance.LaunchCurseRoomEffect();
            while (!RoomEffectManager.Instance.RoomEffectDone())
                yield return null;
        }
    }
}
