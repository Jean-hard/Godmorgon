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

    private void HandSetup()
    {
        //wtf ?
        Transform[] cardsInHand = hand.gameObject.GetComponentsInChildren<Transform>();   // tableau contenant les cartes en main --> TODO: les récup du gameengine
        foreach (Transform _card in cardsInHand)
        {
            cardDisplay = _card.GetComponent<CardDisplay>();
            if(cardDisplay != null)
            {
                //cardDisplay.onCardDragBeginDelegate += OnCardDragBegin;
                //cardDisplay.onCardDragDelegate += OnCardDrag;
                //cardDisplay.onCardDragEndDelegate += OnCardDragEnd;
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

    private void OnCardDragBegin(CardDisplay choosedCard, PointerEventData eventData)
    {
        //Debug.Log("on drag begin " + card.name);
        selectedCard = choosedCard.card;
    }

    private void OnCardDrag(CardDisplay card, PointerEventData eventData)
    {
        //Debug.Log("on drag " + card.name);

    }

    private void OnCardDragEnd(CardDisplay choosedCard, PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<CardDisplay>().card.GetType().Name == "MoveCard")
        {
            bool moveValidate = player.GetComponent<PlayerMove>().UseMoveCard();
            if(moveValidate)
                Destroy(choosedCard.gameObject);
            //Debug.Log("La carte move est droppée");
        }
    }

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
