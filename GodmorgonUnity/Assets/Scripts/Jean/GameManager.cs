using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using GodMorgon.Models;
using GodMorgon.StateMachine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerGUI playerGUI = null;

    [SerializeField]
    private GameObject hand = null;

    [SerializeField]
    private HandManager handManager = null;

    private GameObject player;

    //button pour passer au tour du player, DEVRA DISPARAITRE
    public GameObject playerTurnButton;

    #region Singleton Pattern
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }

            return instance;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        
    }

    #region IN-GAME BUTTON FUNCTION
    /**
     * Draw a card from the player Deck
     */
    public void DrawCardButton()
    {
        //Check if the max hand capability is not reach yet.
        if (GameEngine.Instance.hand.Count() < GameEngine.Instance.GetSettings().MaxHandCapability)
        {
            BasicCard cardDrawn = GameEngine.Instance.DrawCard();
            handManager.AddCard(cardDrawn);
        }
        else
            Debug.Log("capacité de carte maximale");
    }

    /**
     * Button to pass the InitializationState and go to PlayerTurn state
     */
    public void StartPlayerTurn()
    {
        GameEngine.Instance.SetState(StateMachine.STATE.PLAYER_TURN);
        playerTurnButton.SetActive(false);
    }

    #endregion
}
