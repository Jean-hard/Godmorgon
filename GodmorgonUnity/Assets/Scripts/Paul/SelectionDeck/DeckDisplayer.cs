using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GodMorgon.Models;

namespace GodMorgon.DeckBuilding.SelectionDeck
{
    /**
     * this class manages the deck display for the deck selection phase
     */
    public class DeckDisplayer : MonoBehaviour
    {
        /**
         * Text zone for the name and description of the selected deck
         */
        [SerializeField]
        private Text deckName = null;
        [SerializeField]
        private Text deckDescription = null;

        /**
         * example card with basic information to reset the deck display
         */
        [SerializeField]
        private BasicCard baseCard = null;

        /**
         * GameObject list of the card contain in the ScrollView.
         */
        [SerializeField]
        private List<CardDisplay> cardsDisplayed = null;

        /**
         * updates all cards displayed relative to the card in the selected deck
         */
        public void UpdateCards(DeckContent deck)
        {
            deckName.text = deck.name;
            deckDescription.text = deck.description;

            for(int i = 0; i < cardsDisplayed.Count; i++)
            {
                //exit the loop if all the card of the deck have been checked
                if (i < deck.cards.Count)
                    cardsDisplayed[i].UpdateCard(deck.cards[i]);
                else
                    return;
            }
        }

        /**
         * resets the deck display using the example base card.
         * call when the return button is pressed
         */
        public void ResetCards()
        {
            deckName.text = "deck name";
            deckDescription.text = "desck description";

            for (int i = 0; i < cardsDisplayed.Count; i++)
            {
                cardsDisplayed[i].UpdateCard(baseCard);
            }
        }

        /**
         * TODO : "UpdateCards" doit pouvoir instantier des cartes si le deck contient plus de cartes
         * qu'il n'y a de gameObject.
         * Pour cela, prévoir un layout group vertical et des Layout group horizontal
         */
    }
}
