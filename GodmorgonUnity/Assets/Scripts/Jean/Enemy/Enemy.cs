using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Models
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
    public class Enemy : ScriptableObject
    {
        public string enemyId;
        public int health;
        public Sprite skin;
        public int nbMoves = 1;
        public float speed = 5;
        public int attack = 0;
        public int defense = 0;
    }
}