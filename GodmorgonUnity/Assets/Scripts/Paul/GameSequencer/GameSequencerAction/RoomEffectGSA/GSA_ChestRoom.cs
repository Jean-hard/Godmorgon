using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodMorgon.CardEffect;

namespace GodMorgon.GameSequencerSpace
{
    public class GSA_ChestRoom : GameSequencerAction
    {
        public override IEnumerator ExecuteAction(GameContext context)
        {
            RoomEffectManager.Instance.LaunchChestRoomEffect();
            while (!RoomEffectManager.Instance.RoomEffectDone())
                yield return null;
        }
    }
}
