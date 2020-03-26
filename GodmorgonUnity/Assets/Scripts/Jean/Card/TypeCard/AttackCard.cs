using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Models
{
    /**
     * Classe de la carte de type attaque
     * Elle hérite de BasicCard qui contient ses infos de base (nom, description, ...)
     * Elle contient ses effets propres à elle
     */
    [CreateAssetMenu(fileName = "New Card", menuName = "Cards/AttackCard")]
    public class AttackCard : BasicCard
    {
        public float attackValue;
    }
}

