using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.DeckBuilding;
using GodMorgon.StateMachine;


/**
 * Class that initialize data like settings and truly launch the game ---- FROM DRAFT SCENE !
 */
public class BootStrap : MonoBehaviour
{
    //Settings parameter for GameEngine
    [SerializeField]
    private GameSettings gameSettings = null;

    /**
     * Set the game scene for test
     */
    [SerializeField]
    private string gameSceneName = null;

    //initialize data at start and launch first game function
    public void Start()
    {
        if (GameEngine.Instance.gameLaunched == false)
        {
            GameEngine.Instance.SetSettings(gameSettings);
            GameEngine.Instance.SetStartingGame();
            GameEngine.Instance.SetCurrentGameScene(gameSceneName);

            GameEngine.Instance.SetState(StateMachine.STATE.DRAFT);
        }
    }
}
