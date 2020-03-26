﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Models
{

    /**
     * Classe mère des types de carte (attack, defense, move, ...)
     * Elle contient des variables communes à chaque type de carte
     * Elle permet que toutes les classes filles soient de scriptable object
     */
    [CreateAssetMenu(fileName = "New Card", menuName = "Cards/BasicCard")]
    public class BasicCard : ScriptableObject
    {
        public int id;
        public new string name;
        public string description;
        public int actionCost = 1;

        public Sprite template;
        public Sprite artwork;
    }
}
