using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.DeckBuilding;


/**
 * Class that initialize data like settings and truly launch the game
 */
public class BootStrap : MonoBehaviour
{
    //Settings parameter for GameEngine
    [SerializeField]
    private GameSettings gameSettings;

    //initialize data at start and launch first game function
    public void Start()
    {
        GameEngine.Instance.SetSettings(gameSettings);

        ///actually the game start with this :
        DeckBuildingManager.Instance.DraftStart();
    }
}
