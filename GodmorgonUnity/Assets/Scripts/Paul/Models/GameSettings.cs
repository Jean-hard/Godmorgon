using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Models
{
    [CreateAssetMenu(fileName = "Settings", menuName = "AddSettings")]
    public class GameSettings : ScriptableObject
    {
        /**
        * Maximum number of cards in Hand. 
        * Must be tweek.
        */
        public int MaxHandCapability = 5;

        public DeckContent GameDeck;
    }

}