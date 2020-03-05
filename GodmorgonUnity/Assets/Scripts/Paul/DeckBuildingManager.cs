﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.DeckBuilding.SelectionDeck;
using GodMorgon.DeckBuilding.Draft;
using GodMorgon.Models;

namespace GodMorgon.DeckBuilding
{
    public class DeckBuildingManager : MonoBehaviour
    {
        /**
         * This class will be called several times by the managers of the different phases
         * because it will store main data for the rest of the game.
         */
        #region SINGLETON PATTERN
        private static DeckBuildingManager _instance;

        public static DeckBuildingManager Instance { get { return _instance; } }
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
        #endregion

        /**
         * link to the manager of the different phase during the Deck Building.
         */
        [SerializeField]
        private DeckSelectPhase deckSelectPhase;
        [SerializeField]
        private DraftPhase draftPhase;

        /**
         * Set the GameState for ChooseDeck at start
         * Setup the deckSelectPhase screen.
         * Launch the deck selection phase
         */
        public void DeckSelectionStart()
        {
            GameEngine.Instance.CurrentState = GameEngine.GameState.CHOOSEDECK;
            deckSelectPhase.SetAvailableDeck();
            deckSelectPhase.gameObject.SetActive(true);
        }

        /**
         * Launch the draft phase and so finish the deck selection phase
         */
        public void DraftStart()
        {
            GameEngine.Instance.CurrentState = GameEngine.GameState.DRAFTING;
            deckSelectPhase.gameObject.SetActive(false);
            draftPhase.gameObject.SetActive(true);
            draftPhase.StartDraftSequence();
        }

        /**
         * Set the player deck
         */
        public void SetPlayerDeck(DeckContent deckSelected)
        {
            GameEngine.Instance.SetPlayerDeck(deckSelected);
            Debug.Log("player deck set, player deck = " + GameEngine.Instance.playerDeck);
        }

        //Add a card to the player Deck (normaly only use by the draftPhase)
        public void AddCardToPlayerDeck(BasicCard cardToAdd)
        {
            //Debug.Log(GameEngine.Instance.playerDeck);
            GameEngine.Instance.playerDeck.cards.Add(cardToAdd);
        }
    }
}
