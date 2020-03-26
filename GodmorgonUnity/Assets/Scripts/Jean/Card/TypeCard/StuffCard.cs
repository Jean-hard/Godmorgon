using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Models
{
    /**
     * Classe de la carte de type stuff (équipement)
     * Elle hérite de BasicCard qui contient ses infos de base (nom, description, ...)
     * Elle contient ses effets propres à elle
     */
    [CreateAssetMenu(fileName = "New Card", menuName = "Cards/StuffCard")]
    public class StuffCard : BasicCard
    {
        public int lifeBonus = 10;
        public int powerBonus = 5;

        public StuffTypes StuffType;
        public enum StuffTypes
        {
            HAT,
            CHEST,
            PANTS,
            SHOES
        }
    }
}
