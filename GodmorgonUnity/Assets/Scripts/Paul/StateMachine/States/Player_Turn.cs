using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Sound;

namespace GodMorgon.StateMachine
{
    public class Player_Turn : State
    {
        public override void OnStartState()
        {
            //Debug.Log("On Player turn State");
            GameManager.Instance.DownPanelBlock(false);
            GameManager.Instance.UpdateCardDataDisplay();
            GameManager.Instance.ShowPlayerTurnImage();
            GameManager.Instance.UnlockDragCardHandler(true);

        }
    }
}