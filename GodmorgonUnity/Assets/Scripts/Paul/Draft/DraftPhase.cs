using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GodMorgon.Models;
using GodMorgon.StateMachine;

namespace GodMorgon.DeckBuilding.Draft
{
    public class DraftPhase : MonoBehaviour
    {
        //Deck use to draw card for the draft
        [SerializeField]
        private DeckContent draftDeck = null;

        /**
         * link for the three card to choose during the draft
         */
        [SerializeField]
        private List<CardDisplay> cardsOnDraft = null;

        /**
         * Number of draft to execute
         */
        [SerializeField]
        private int nbDraftLeft = 5;

        /**
         * text indiquant le nombre de séquence de draft restante
         * ----Temporaire------
         */
        public Text nbDraftLeftText = null;

        /**
         * Start a draft sequence
         * choose 5 card randomly from draft deck and display them on screen
         */
        public void StartDraftSequence()
        {
            if (nbDraftLeft > 0)
            {
                int rdmCardNb;
                for (int i = 0; i < cardsOnDraft.Count; i++)
                {
                    rdmCardNb = Random.Range(0, draftDeck.cards.Count);
                    cardsOnDraft[i].UpdateCard(draftDeck.cards[rdmCardNb]);
                }
            }
            //after pick all the card, we shuffle the deck and start the load of the game scene
            else
            {
                GameEngine.Instance.ShufleCompleteDeck();
                GameEngine.Instance.SetState(StateMachine.StateMachine.STATE.LOAD);
            }
            //Debug.Log("Phase de draft Complete, Lancement de la scene de jeu avec le deck complet");
        }

        /**
         * button function
         * Add the card choose durring the draft to the deck player.
         * And reload the draft sequence
         */
        public void ChooseCard(CardDisplay cardChoosed)
        {
            DeckBuildingManager.Instance.AddCardToPlayerDeck(cardChoosed.card);
            nbDraftLeft -= 1;
            nbDraftLeftText.text = nbDraftLeft.ToString();
            StartDraftSequence();
        }
    }
}
