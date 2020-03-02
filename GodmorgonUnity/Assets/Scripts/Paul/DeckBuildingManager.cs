﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.DeckBuilding.SelectionDeck;
using Game.DeckBuilding.Draft;
using Game.component;

namespace Game.DeckBuilding
{
    public class DeckBuildingManager : MonoBehaviour
    {
        /**
         * This class will be called several times by the managers of the different phases
         * because it will store main data for the rest of the game.
         * To facilitate calls to this class we create a singleton.
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
         * Deck choose during the deck selection phase
         */
        private DeckContent playerDeck;


        /**
         * Launch the deck selection phase
         */
        public void DeckSelectionStart()
        {
            deckSelectPhase.gameObject.SetActive(true);
        }

        /**
         * Launch the draft phase and so finish the deck selection phase
         */
        public void DraftStart()
        {
            deckSelectPhase.gameObject.SetActive(false);
            draftPhase.gameObject.SetActive(true);
        }

        /**
         * Set the player deck
         */
        public void SetPlayerDeck(DeckContent deckSelected)
        {
            playerDeck = deckSelected;
        }
    }
}
