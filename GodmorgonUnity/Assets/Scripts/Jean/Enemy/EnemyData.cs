using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Enemy
{
    public class EnemyData : Entity
    {
        public string enemyId = null;
        public int health = 0;
        public int attack = 0;
        public int defense = 0;
        public int nbMoves = 0;
        public float speed = 0;
        public Sprite skin = null;
        public bool inPlayersRoom = false;
        public EnemyView enemyView = null;
        
    }
}
