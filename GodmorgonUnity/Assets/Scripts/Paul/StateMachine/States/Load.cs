using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//////   ?????????????
using GodMorgon.IniatializationScene;

namespace GodMorgon.StateMachine
{
    /**
     * Transition state beetween draft scene and gameScene
     */
    public class Load : State
    {
        public override void OnStartState()
        {
            Debug.Log("On Load State");

            SceneManager.LoadScene(GameEngine.Instance.currentGameScene);

            //Time to secure good initialization of the gameobject of the game scene...
            IniatializationScene.InitializationScene.Instance.LaunchTimerGameScene();
        }
    }
}
