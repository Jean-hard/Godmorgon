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

    //button pour passer au tour du player, DEVRA DISPARAITRE
    public GameObject playerTurnButton;

    #region Singleton Pattern
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    /**
     * At the beginning of turn,
     * the player discard all his currents cards and draw new one.
     */
    public void PlayerDraw()
    {
        handManager.DiscardAllCard();
        for(int i = 0; i < GameEngine.Instance.GetSettings().MaxHandCapability; i++)
        {
            BasicCard cardDrawn = GameEngine.Instance.DrawCard();
            handManager.AddCard(cardDrawn);
        }
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

    public void StartRingmasterTurn()
    {
        GameEngine.Instance.SetState(StateMachine.STATE.RINGMASTER_TURN);
        //playerTurnButton.SetActive(false);
    }

    #endregion

    /**
    * Discard the card
    * Call by the handManager
    */
    public void DiscardHandCard(BasicCard cardDiscarded)
    {
        GameEngine.Instance.DiscardCard(cardDiscarded);
    }
}
