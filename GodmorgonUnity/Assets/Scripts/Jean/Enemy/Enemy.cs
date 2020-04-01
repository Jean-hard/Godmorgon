using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Models
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
    public class Enemy : ScriptableObject
    {
        public int id;
        public new string name;
        public int health;
        public Sprite skin;
        public int nbMoves;
        public int attack = 0;
        public int defense = 0;
        public int movement = 0;
    }
}