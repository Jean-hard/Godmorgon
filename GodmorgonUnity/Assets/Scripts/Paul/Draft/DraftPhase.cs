using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.DeckBuilding.Draft
{
    public class DraftPhase : MonoBehaviour
    {
        /**
         * link for the three card gameObject to choose during the draft
         */
        [SerializeField]
        private GameObject card1;
        [SerializeField]
        private GameObject card2;
        [SerializeField]
        private GameObject card3;

        /**
         * Number of draft to execute
         */
        [SerializeField]
        private int nbDraftLeft = 5;

        /**
         * text indiquant le nombre de séquence de draft restante
         * ----Temporaire------
         */
        public Text nbDraftLeftText;
    }
}
