using System.Collections;
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
        private DraftPhase draftPhase = null;

        /**
         * Launch the draft phase and so finish the deck selection phase
         */
        public void DraftStart()
        {
            draftPhase.gameObject.SetActive(true);
            draftPhase.StartDraftSequence();
        }

        //Add a card to the player Deck (normaly only use by the draftPhase)
        public void AddCardToPlayerDeck(BasicCard cardToAdd)
        {
            //Debug.Log(GameEngine.Instance.playerDeck);
            GameEngine.Instance.playerDeck.AddCard(cardToAdd);
        }
    }
}
