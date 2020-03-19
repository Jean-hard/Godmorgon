using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace GodMorgon.StateMachine
{
    public class Initialization_Maze : State
    {
        /**
         * At start of this state we launch the game scene
         */
        public override void OnStartState()
        {
            SceneManager.LoadScene(GameEngine.Instance.currentGameScene);
            Debug.Log("le deck fait à présent : " + GameEngine.Instance.playerDeck.Count());
        }
    }
}