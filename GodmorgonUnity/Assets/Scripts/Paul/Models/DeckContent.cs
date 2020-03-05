using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GodMorgon.Models
{
    [CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]

    /**
     * Contain the list of card forming this deck
     */
    public class DeckContent : ScriptableObject
    {
        public int id;
        public new string name;
        public string description;
        public Image artwork;

        public List<BasicCard> cards;
    }
}
