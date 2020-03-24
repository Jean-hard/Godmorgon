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
    private PlayerGUI playerGUI;

    [SerializeField]
    private GameObject hand;

    [SerializeField]
    private HandManager handManager;

    private GameObject player;

    //wtf ?
    CardDisplay cardDisplay;

    //wtf ?
    public BasicCard selectedCard;

    //ça sert à quoi ?
    private bool isHandUpdated;

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

    /**
     * Set l'écoute du comportement des cartes de la main pour pouvoir ajouter des évènements dans le GameManager lors du drag/drop d'une carte
     * TODO : Lancer après la distribution d'une carte
     */
    private void ListenHandCardsBehaviour()
    {
        // tableau contenant les cartes en mainsur la scène
        List<BasicCard> cardsInHand = GameEngine.Instance.GetHandCards();        
        //parcourt toutes les cartes de la main pour y lier les fonctions disponibles ici lors du drag and drop de ces cartes
        foreach (BasicCard _card in cardsInHand)
        {
            cardDisplay.onCardDragBeginDelegate += OnCardDragBegin; //on ajoute pour chaque carte une fonction à la liste de celles lancées au début du drag
            cardDisplay.onCardDragDelegate += OnCardDrag;   //on ajoute pour chaque carte une fonction à la liste de celles lancées pendant le drag
            cardDisplay.onCardDragEndDelegate += OnCardDragEnd; //on ajoute pour chaque carte une fonction à la liste de celles lancées à la fin du drag
        }
    }

    //fonction lancée au drag d'une carte
    private void OnCardDragBegin(GameObject draggedCard, PointerEventData eventData)
    {

    }

    //fonction lancée lorsqu'on a une carte en main
    private void OnCardDrag(GameObject draggedCard, PointerEventData eventData)
    {
        
    }

    //fonction lancée au drop d'une carte
    private void OnCardDragEnd(GameObject draggedCard, PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<CardDisplay>().card.GetType().Name == "MoveCard")
        {
            //supprime la case si la carte est droppée dans une case voisine de celle du player
            bool moveValidate = player.GetComponent<PlayerMove>().UseMoveCard();
            if (moveValidate)
            {
                GameEngine.Instance.DiscardCard(eventData.pointerDrag.GetComponent<CardDisplay>().card);   //on place la carte dans la disposal pile une fois utilisée
                Destroy(draggedCard);   // WIP : retirer de la main plutot que détruire
            }
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

    #endregion
}
