using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Timeline;
using GodMorgon.Sound;
using GodMorgon.Enemy;

namespace GodMorgon.StateMachine
{
    /**
     * state where the ringmaster play his action
     */
    public class RingMaster_Turn : State
    {
        public override void OnStartState()
        {
            //Debug.Log("On RingMaster turn State");
            GameManager.Instance.DownPanelBlock(true);
            GameManager.Instance.ShowRingmasterTurnImage();
            EnemyManager.Instance.UpdateEnemiesInSameRoom();
            GameManager.Instance.UnlockDragCardHandler(false);

            //TimelineManager.Instance.DoAction();
        }
    }
}