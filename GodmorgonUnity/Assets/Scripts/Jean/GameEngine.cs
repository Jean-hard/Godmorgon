using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using GodMorgon.Models;
using GodMorgon.CardContainer;
using GodMorgon.StateMachine;

public class GameEngine
{
    /**
     * FOR BOOTSTRAP
     * check if the game is already launch
     */
    public bool gameLaunched = false;

    /**
     * FOR BOOTSTRAP
     * the current scene of game to play
     */
    public string currentGameScene;

    /**
     * Current Game Settings
     */
    private GameSettings settings;

    //Current Playing Deck.
    public Deck playerDeck;

    /**
     * The hand card container 
     */
    public Hand hand;

    /**
     * The discard card container
     */
    public DisposalPile disposalPile;

    /**
     * The State Machine use by the gameEngine
     */
    public StateMachine stateMachine;

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

    //Set settings from bootStrap
    public void SetSettings(GameSettings theSettings)
    {
        settings = theSettings;
    }

    //return the game settings
    public GameSettings GetSettings()
    {
        return settings;
    }

    /**
     * Private Constructor for Singleton.
     * Initialize the list and the stateMachine
     */
    private GameEngine ()
    {

        playerDeck = new Deck();
        hand = new Hand();
        disposalPile = new DisposalPile();
        stateMachine = new StateMachine();
    }

    /**
     * Set the new state of the game
     */
    public void SetState(StateMachine.STATE newState)
    {
        stateMachine.SetState(newState);
    }

    /**
     * Get the current state
     */
    public StateMachine.STATE GetState()
    {
        return stateMachine.GetCurrentState();
    }

    /**
     * Restart the current state
     */
    public void RestartState()
    {
        stateMachine.RestartState();
    }

    //Set Player deck content
    public void SetPlayerDeck(DeckContent theDeckChoosed)
    {
        //Debug.Log("le deck sélectionner fait : " + theDeckChoosed.cards.Count);
        foreach (BasicCard card in theDeckChoosed.cards)
        {
            AddCardToPlayerDeck(card);
        }
        //Debug.Log("le deck fait maintenant : " + playerDeck.Count());
    }

    /**
     * Set the playerDeck and shuffle it
     */
    public void SetStartingGame()
    {
        //Debug.Log("Game set");
        //deck for the shuffle
        Deck tempDeck = new Deck();

        SetPlayerDeck(settings.GameDeck);
        //Debug.Log(tempDeck.GetCards().Count);

        while (playerDeck.Count() > 0)
        {
            tempDeck.AddCard(playerDeck.DrawCard());
        }
        playerDeck = tempDeck;
        //Debug.Log(playerDeck.GetCards().Count);

        gameLaunched = true;
    }

    //Shufle the deck after the draft state
    public void ShufleCompleteDeck()
    {
        Deck tempDeck = new Deck();
        while (playerDeck.Count() > 0)
        {
            tempDeck.AddCard(playerDeck.DrawCard());
        }
        playerDeck = tempDeck;
    }

    /**
     * Set the current scene of game
     */
    public void SetCurrentGameScene(string gameSceneName)
    {
        if (gameSceneName == "")
            currentGameScene = "GameScene";
        else
        currentGameScene = gameSceneName;
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
        //if (hand.Count() >= settings.MaxHandCapability /*+ player.HandOverflow*/)//important pour le player
        //    throw new HandIsFullException();
        if (playerDeck.Count() == 0)
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

    //Add a card directly to the disposal pile (Discard)
    public void AddCardToDiscardPile(BasicCard discardedCard)
    {
        if (discardedCard != null)
            disposalPile.AddCard(discardedCard);
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

    /**
     * Get the cards in the hand
     */
    public List<BasicCard> GetHandCards()
    {
        return hand.GetCards();
    }
}
