using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GodMorgon.Models;

namespace GodMorgon.DeckBuilding.SelectionDeck
{
    /**
     * Obsolète
     */
    public class DeckSelectPhase : MonoBehaviour
    {
        //list of deck displayed on screen.
        [SerializeField]
        private List<DeckDisplay> deckDisplayed;

        //list of Button on screen
        [SerializeField]
        private List<Button> SeeCardButtons;
         

        //Scroll view that show the card of the selected deck
        [SerializeField]
        private DeckDisplayer deckDisplayer;

        /**
         * Stored the current deck selected
         */
        private DeckContent currentDeck;

        /**
         * Set the available deck to the deck gameObject on screen.
         * And Set the button for the right deck.
         */
        //public void SetAvailableDeck()
        //{
        //    for(int i = 0; i < 3; i++)
        //    {
        //        Debug.Log(deckDisplayed[i].name);
        //        deckDisplayed[i].UpdateDeck(GameEngine.Instance.availableDecks[i]);
        //    }

        //    // assigns the right function to the linked deck
        //    SeeCardButtons[0].onClick.AddListener(delegate
        //    {ShowDeckCard(deckDisplayed[0].deck);});
        //    SeeCardButtons[1].onClick.AddListener(delegate      //======= ALED =========
        //    { ShowDeckCard(deckDisplayed[1].deck); });
        //    SeeCardButtons[2].onClick.AddListener(delegate
        //    { ShowDeckCard(deckDisplayed[2].deck); });
        //}

//#region function for button

//        /**
//         * Read the list of card contain in the deck and update the card in the panel.
//         */
//        public void ShowDeckCard(DeckContent deck)
//        {
//            deckDisplayer.gameObject.SetActive(true);
//            deckDisplayer.UpdateCards(deck);
//            currentDeck = deck;
//        }

//        /**
//         * Hide the panel
//         */
//        public void HideDeckCard()
//        {
//            deckDisplayer.ResetCards();
//            deckDisplayer.gameObject.SetActive(false);
//        }

//        /**
//         * Save the deck selected and launch the draft phase.
//         */
//        public void ValidateDeck()
//        {
//            DeckBuildingManager.Instance.SetPlayerDeck(currentDeck);
//            DeckBuildingManager.Instance.DraftStart();
//        }
//#endregion
    }
}

/**
 * TODO :
 * la liste de deck pourra contenir plus de 3 éléments à choisir ----PEUT ETRE---
 */
