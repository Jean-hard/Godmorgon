using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using GodMorgon.Models;
using GodMorgon.CardContainer;

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

    //// Current Playing Deck.
    public Deck playerDeck;

    /**
     * The card container list
     */
    public Hand hand;

    public DisposalPile disposalPile;


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
        playerDeck = new Deck();
        hand = new Hand();
        disposalPile = new DisposalPile();
    }

    //Set settings from bootStrap or else
    public void SetSettings(GameSettings theSettings)
    {
        settings = theSettings;
    }

    //Set Player deck content
    public void SetPlayerDeck(DeckContent theDeckChoosed)
    {
        foreach (BasicCard card in theDeckChoosed.cards)
        {
            Debug.Log(card);
            playerDeck.AddCard(card);
        }
    }

    public enum GameState
    {
        CHOOSEDECK,     //Le joueur doit choisir un deck
        DRAFTING,       //Le joueur doit choisir une carte parmi 3
        STARTGAME,      //Arrivé sur le plateau de jeu
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

            case GameState.STARTGAME:
                // Lorsque le joueur doit choisir quelle carte il joue
                SetStartGameMode();
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
    //FOR DEBUG
    public void InitializePlayerDeck()
    {
        playerDeck = new Deck();
    }


    //clear all the card in the player deck
    public void ClearPlayerDeck()
    {
        playerDeck.ClearCards();
    }

    //draw the card on top of the player deck and remove it from the deck
    public BasicCard DrawCard()
    {
        // Check if We Can!
        if (hand.Count() >= settings.MaxHandCapability /*+ player.HandOverflow*/)//important pour le player
            throw new HandIsFullException();
        else if (playerDeck.Count() == 0)
            throw new DeckIsEmptyException();

        else
        {
            // Take from the deck
            BasicCard myNewCard = playerDeck.DrawCard();
            hand.AddCard(myNewCard);
            return myNewCard;
        }
    }

    //Add card to the deck of the player
    public void AddCardToPlayerDeck(BasicCard newCard)
    {
        playerDeck.AddCard(newCard);
    }

    //Takes a card from the hand and junk it to the disposal pile (Discard)
    public void DiscardCard(BasicCard toRemove = null)
    {
        if (toRemove == null)
            DiscardRandomCard();
        else
        {
            BasicCard discarded = hand.DrawCard(toRemove);
            if (discarded != null)
                disposalPile.AddCard(discarded);
        }
    }

    //Takes a random card from the hand and junk it to the disposal pile
    public void DiscardRandomCard()
    {
        BasicCard discarded = hand.DrawCard();
        if (discarded != null)
            disposalPile.AddCard(discarded);
    }
    
    // Moves the card from the disposal, randomly into the desk
    public void ShuffleDeck()
    {
        // The desck MUST be empty before to be shuffled with
        // the disposal pile :)
        if (playerDeck.Count() == 0)
        {
            while (disposalPile.Count() > 0)
            {
                playerDeck.AddCard(disposalPile.DrawCard());
            }
        }
    }

    #endregion

    private void SetChooseDeckMode()
    {
        // On commence par charger les decks préconstruits
        foreach (DeckContent unDeck in settings.decksPreconstruits)
            AddDeck(unDeck);
    }

    /**
     * Set the playerDeck and shuffle it
     */
    private void SetStartGameMode()
    {
        Deck tempDeck = new Deck();
        SetPlayerDeck(settings.GameDeck);

        while (playerDeck.Count() > 0)
        {
            tempDeck.AddCard(playerDeck.DrawCard());
        }
        Debug.Log(playerDeck.Count());
        playerDeck = tempDeck;
        Debug.Log(playerDeck.Count());
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

    /**
     * Get the cards in the hand
     */
    public List<BasicCard> GetHandCards()
    {
        return hand.GetCards();
    }
}
