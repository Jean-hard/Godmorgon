using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using GodMorgon.Timeline;
using GodMorgon.StateMachine;

namespace GodMorgon.StateMachine
{
    public class Initialization_Maze : State
    {
        /**
         * At start of this state we launch the game scene
         */
        public override void OnStartState()
        {
            //Debug.Log("On Initialization maze State");
            //Debug.Log("le deck fait à présent : " + GameEngine.Instance.playerDeck.Count());

            //set the timeline
            TimelineManager.Instance.InitTimeline();

            //Set the player hand
            GameManager.Instance.PlayerDraw();

            //Start the player turn
            GameEngine.Instance.SetState(StateMachine.STATE.PLAYER_TURN);
        }
    }
}