﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.StateMachine;

/**
 * Class that initialize data like settings and truly launch the game ---- FROM GAME SCENE !
 */
public class BootStrapGameScene : MonoBehaviour
{
    //Settings parameter for GameEngine
    [SerializeField]
    private GameSettings gameSettings;

    //initialize data at start and launch first game function
    public void Start()
    {
        if (GameEngine.Instance.gameLaunched == false)
        {
            GameEngine.Instance.SetSettings(gameSettings);
            GameEngine.Instance.SetState(StateMachine.STATE.INITIALIZATION_MAZE);

            GameEngine.Instance.SetStartingGame();
        }
    }
}
