using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Models
{
    [CreateAssetMenu(fileName = "Settings", menuName = "AddSettings")]
    public class GameSettings : ScriptableObject
    {
        // Des trucs

        // Des machins

        // Les decks disponibles pour démarrer le jeu
        public List<DeckContent> decksPreconstruits = new List<DeckContent>();

    }

}