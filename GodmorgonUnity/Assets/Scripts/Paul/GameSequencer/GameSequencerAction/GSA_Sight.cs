using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodMorgon.CardEffect;

/**
 * Action de sight
 */
namespace GodMorgon.GameSequencerSpace
{
    public class GSA_Sight : GameSequencerAction
    {
        public override IEnumerator ExecuteAction(GameContext context)
        {
            Vector3Int targetRoomPos = new Vector3Int(context.targetRoom.x, context.targetRoom.y, 0);
            FogMgr.Instance.RevealRoomAtPosition(targetRoomPos, context.card.effectsData[0].sightRange);
            //FogMgr.Instance.RevealAtPosition(targetRoomPos);
            while (!FogMgr.Instance.RevealDone())
                yield return null;
        }
    }
}