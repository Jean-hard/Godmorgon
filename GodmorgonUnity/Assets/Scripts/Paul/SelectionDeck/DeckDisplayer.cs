using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.component;

namespace Game.SelectionDeck
{
    public class DeckDisplayer : MonoBehaviour
    {
        /**
         * Text zone for the name and description of the selected deck
         */
        [SerializeField]
        private Text deckName;
        [SerializeField]
        private Text deckDescription;

        /**
         * GameObject list of the card contain in the ScrollView.
         */
        [SerializeField]
        private List<CardDisplay> cardsDisplayed;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * updates all cards displayed relative to the card in the selected deck
         */
        public void UpdateCards(DeckContent deck)
        {
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
         * TODO : "UpdateCards" doit pouvoir instantier des cartes si le deck contient plus de cartes
         * qu'il n'y a de gameObject.
         * Pour cela, prévoir un layout group vertical et des Layout group horizontal
         */
    }
}
