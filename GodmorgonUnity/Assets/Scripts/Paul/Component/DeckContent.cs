using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public List<BasicCard> cards;
    }
}
