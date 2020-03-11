using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GodMorgon.Models;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerGUI playerGUI;

    [SerializeField]
    private GameObject hand;

    private GameObject player;

    //wtf ?
    CardDisplay cardDisplay;

    //wtf ?
    public BasicCard selectedCard;

    //ça sert à quoi ?
    private bool isHandUpdated;

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
        //ça marche comment ?
        if (!isHandUpdated)
        {
            HandSetup();
            isHandUpdated = true;
        }
    }

    /**
     * Set l'écoute du comportement des cartes de la main
     */
    private void HandSetup()
    {
        // tableau contenant les cartes en main SUR LA SCENE --> TODO: les récup du gameengine
        Transform[] cardsInHand = hand.gameObject.GetComponentsInChildren<Transform>();   
        
        //parcourt toutes les cartes de la main pour y lier les fonctions disponibles ici lors du drag and drop de ces cartes
        foreach (Transform _card in cardsInHand)
        {
            cardDisplay = _card.GetComponent<CardDisplay>();
            if(cardDisplay != null)
            {
                cardDisplay.onCardDragBeginDelegate += OnCardDragBegin; //on ajoute une fonction à la liste de celles lancées au début du drag
                cardDisplay.onCardDragDelegate += OnCardDrag;   //on ajoute une fonction à la liste de celles lancées pendant le drag
                cardDisplay.onCardDragEndDelegate += OnCardDragEnd; //on ajoute une fonction à la liste de celles lancées à la fin du drag
            }
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

    /**
     * update the card gameObject in the Hand
     */
    //public void UpdateHand()
    //{
    //    foreach
    //}

    #region IN-GAME BUTTON FUNCTION
    /**
     * Draw a card from the player Deck
     */
    public void DrawCardButton()
    {
        GameEngine.Instance.DrawCard();
    }

    #endregion
}
