using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.component;

namespace Game.DeckBuilding.SelectionDeck
{
    public class DeckSelectPhase : MonoBehaviour
    {
        //Scroll view that show the card of the selected deck
        [SerializeField]
        private DeckDisplayer deckDisplayer;

        /**
         * Stored the current deck selected
         */
        private DeckContent currentDeck;

#region function for button

        /**
         * Read the list of card contain in the deck and update the card in the panel.
         */
        public void ShowDeckCard(DeckContent deck)
        {
            deckDisplayer.gameObject.SetActive(true);
            deckDisplayer.UpdateCards(deck);
        }

        /**
         * Hide the panel
         */
        public void HideDeckCard()
        {
            deckDisplayer.ResetCards();
            deckDisplayer.gameObject.SetActive(false);
        }

        /**
         * Save the deck selected and launch the draft phase.
         */
        public void ValidateDeck()
        {
            DeckBuildingManager.Instance.SetPlayerDeck(currentDeck);
            DeckBuildingManager.Instance.DraftStart();
        }
#endregion
    }
}
