using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodMorgon.Models;
using UnityEngine.EventSystems;

public class GameEngine
{
    /**
     * Current Game State.
     */
    private GameState currentState = GameState.DEFAULT;

    /**
     * Current Game Settings
     */
    private GameSettings settings;

    // List of Deck to choose from.
    public List<DeckContent> availableDecks = new List<DeckContent>();

    // Current Playing Deck.
    public DeckContent playerDeck;

    //=========================== WIP ===== transfert de GameManager vers GameEngine
    [SerializeField]
    private GameObject hand;

    private GameObject player;

    CardDisplay cardDisplay;

    public BasicCard selectedCard;
    //============================


    #region Singleton Pattern
    private static GameEngine instance;

    public static GameEngine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameEngine();
            }

            return instance;
        }
    }
    #endregion

    /**
     * Private Constructor for Singleton.
     */
    private GameEngine ()
    {
        playerDeck = DeckContent.CreateInstance<DeckContent>();
    }

    //Set settings from bootStrap or else
    public void SetSettings(GameSettings theSettings)
    {
        settings = theSettings;
    }

    //Set Player deck
    public void SetPlayerDeck(DeckContent theDeckChoosed)
    {
        playerDeck = theDeckChoosed;
    }

    public enum GameState
    {
        CHOOSEDECK,     //Le joueur doit choisir un deck
        DRAFTING,       //Le joueur doit choisir une carte parmi 3
        PLAYING,        //Au tour du joueur de jouer
        MOVING,         //Le joueur se déplace
        FIGHTING,       //Le joueur combat un PNJ
        GAMEOVER,       //Le joueur a perdu
        MENU,           //Le joueur est dans le menu principal
        OPTIONS,        //Le joueur est dans le menu option
        DEFAULT
    };

    //permet de get l'état courant, et set l'état courant en lançant ce qu'il doit se passer pour le nouvel état
    public GameState CurrentState
    {
        get
        {
            if (currentState == GameState.DEFAULT)
                Debug.LogError("Il y a un problème de GameState !");

            return currentState;
        }

        set
        {
            //Debug.Log("passage dans le setter");
            if (currentState != value)
            { // Quitte à faire un test avant de changer la valeur, autant ne rien faire du tout si on ne change pas d'état ;)
                currentState = value;
                ApplyStateEffect();
            }
        }
    }
    
    // ========================= Methods

    private void ApplyStateEffect ()
    {

        switch (currentState)
        {
            // Lors du choix du deck
            case GameState.CHOOSEDECK:
                SetChooseDeckMode();

                break;
            case GameState.DRAFTING:
                // Lors de la phase draft
                break;
            case GameState.PLAYING:
                // Lorsque le joueur doit choisir quelle carte il joue
                break;
            case GameState.MOVING:
                // Lorsque le joueur se déplace d'une case à l'autre
                break;
            case GameState.FIGHTING:
                // Lors d'un combat entre joueur et PNJ
                break;
            case GameState.GAMEOVER:
                // Lorsque le joueur est mort
                break;
            case GameState.MENU:
                // Lorsque le joueur est sur le main menu
                break;
            case GameState.OPTIONS:
                // Lorsque le joueur est dans le menu options
                break;
            case GameState.DEFAULT:
                // On ne devrait jamais être dans cet état
                break;
            default:
                // Si on n'est dans aucun de ces états
                break;
        }
    }

    #region CARD MANAGEMENT

    //clear all the card in the player deck
    public void ClearPlayerDeck()
    {
        playerDeck.cards.Clear();
    }

    //draw the card on top of the player deck
    public BasicCard DrawCard()
    {
        return playerDeck.cards[playerDeck.cards.Count];
    }

    //add card to the deck of the player
    public void AddCardToPlayerDeck(BasicCard newCard)
    {
        playerDeck.cards.Add(newCard);
    }

    #endregion

    private void SetChooseDeckMode()
    {
        // On commence par charger les decks préconstruits
        foreach (DeckContent unDeck in settings.decksPreconstruits)
            AddDeck(unDeck);
    }

    // ========================= Deck Management

    /**
     * Adds a new deck to the available decks list.
     */
    public void AddDeck (DeckContent newDeck)
    {
        if (!availableDecks.Contains(newDeck))
            availableDecks.Add(newDeck);
        //Debug.Log("deck ajouté a la liste de deck dispo : " + newDeck.name);
    }

    //================================= WIP ===== Transfert de GameManager vers GameEngine
    private void HandSetup()
    {
        Transform[] cardsInHand = hand.gameObject.GetComponentsInChildren<Transform>();   // tableau contenant les cartes en main --> TODO: les récup du gameengine
        foreach (Transform _card in cardsInHand)
        {
            cardDisplay = _card.GetComponent<CardDisplay>();
            if (null != cardDisplay)
            {
                //cardDisplay.onCardDragBeginDelegate += OnCardDragBegin;
                //cardDisplay.onCardDragDelegate += OnCardDrag;
                cardDisplay.onCardDragEndDelegate += OnCardDragEnd;
            }
        }
    }

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
            //if (moveValidate)
                //Destroy(choosedCard.gameObject);
            //Debug.Log("La carte move est droppée");
        }
    }

    //============================================================================
}
