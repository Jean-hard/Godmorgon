using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.DeckBuilding;

public class BootStrapGameScene : MonoBehaviour
{
    //Settings parameter for GameEngine
    [SerializeField]
    private GameSettings gameSettings;

    //initialize data at start and launch first game function
    public void Start()
    {
        GameEngine.Instance.SetSettings(gameSettings);

        ///actually the game scene start with this :
        GameEngine.Instance.CurrentState = GameEngine.GameState.STARTGAME;
    }
}
