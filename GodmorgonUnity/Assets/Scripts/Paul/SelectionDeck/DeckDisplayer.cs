using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.component;

namespace Game.SelectionDeck
{
    public class DeckDisplayer : MonoBehaviour
    {
        /**
         * GameObject list of the card contain in the ScrollView.
         */
        [SerializeField]
        private List<CardDisplay> cardsDisplay;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateCards(DeckContent deck)
        {

        }
    }
}
