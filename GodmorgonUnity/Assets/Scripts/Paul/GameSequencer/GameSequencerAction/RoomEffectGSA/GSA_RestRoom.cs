using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodMorgon.CardEffect;

/**
 * Effet Rest d'une room
 */
namespace GodMorgon.GameSequencerSpace
{
    public class GSA_RestRoom : GameSequencerAction
    {
        public override IEnumerator ExecuteAction(GameContext context)
        {
            RoomEffectManager.Instance.LaunchRestRoomEffect();
            while (!RoomEffectManager.Instance.RoomEffectDone())
                yield return null;
        }
    }
}