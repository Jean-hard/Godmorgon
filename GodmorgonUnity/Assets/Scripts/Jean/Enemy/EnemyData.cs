﻿using System.Collections;
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

        /**
         * call when enemy is attacked
         * the damage are first applied to the armor and after to the health
         */
        public override void TakeDamage(int damagePoint)
        {
            Debug.Log("enemy healt before was : " + health);
            while(damagePoint > 0 && defense > 0)
            {
                defense--;
                damagePoint--;
            }

            while(damagePoint > 0 && health > 0)
            {
                health--;
                damagePoint--;
            }
            Debug.Log("enemy healt after was : " + health);
        }

    }
}
