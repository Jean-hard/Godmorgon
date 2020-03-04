using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodMorgon.Models;

public class GameEngine
{
    /**
     * Current Game State.
     */
    private GameState currentState;

    /**
     * Current Game Settings
     */
    private GameSettings settings;

    // Si ça c'est pas le deck du joueur (parce que, mettons, on le mettrait dans le player)
    // List of Deck to choose from.
    public List<DeckContent> availableDecks = new List<DeckContent>();

    // Current Playing Deck.
    public DeckContent currentDeck;

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
        // Le GameEngine doit, lors de sa création, commencer
        // par charger les Settings du jeu ! Hop !
        settings = ScriptableObject.CreateInstance<GameSettings>();
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
            if (currentState != value)
            { // Quitte à faire un test avant de changer la valeur, autant ne rien faire du tout si on ne change pas d'état ;)
                currentState = value;

                switch (currentState)
                {
                    // Lors du choix du deck
                    case GameState.CHOOSEDECK:
                        // On commence par charger les decks préconstruits
                        foreach (DeckContent unDeck in settings.decksPreconstruits)
                            AddDeck(unDeck);

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
        }
    }

    // ========================= Deck Management

    /**
     * Adds a new deck to the available decks list.
     */
    public void AddDeck (DeckContent newDeck)
    {
        if (!availableDecks.Contains(newDeck))
            availableDecks.Add(newDeck);
    }
}
