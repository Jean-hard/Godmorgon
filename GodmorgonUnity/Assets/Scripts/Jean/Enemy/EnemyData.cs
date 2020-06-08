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
        public bool inOtherEnemyRoom = false;
        public bool killedByPlayer = false;
        public EnemyView enemyView = null;

        /**
         * call when enemy is attacked
         * the damage are first applied to the armor and after to the health
         */
        public override void TakeDamage(int damagePoint, bool isPlayerAttacking)
        {
            //Debug.Log("enemy health before was : " + health);
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
            //Debug.Log("enemy health after was : " + health);
            UpdateHealthBar();

            float duration = enemyView.enemyHit.GetDuration();    //Récup la durée de la particule
            if (health <= 0)   //Si l'ennemi n'a plus de vie
            {
                if (isPlayerAttacking)
                {
                    killedByPlayer = true;
                    GameManager.Instance.draftPanelActivated = true;    //Met un booleen à true pour faire attendre le séquenceur
                }
                enemyView.KillEnemy(duration);    //On le tue
            }
        }

        //return the position of the enemy
        public override Vector3 GetEntityViewPosition()
        {
            return enemyView.transform.position;
        }

        //update the enemy healthBar
        public override void UpdateHealthBar()
        {
            enemyView.UpdateHealthBar(health, defense);
        }

        //launch hit visual effect
        public override void OnDamage()
        {
            enemyView.OnDamage();
        }

        /**
         * Retourne true si l'ennemi n'a plus de vie
         */
        public override bool IsDead()
        {
            if (health <= 0) return true;

            return false;
        }

        public void AddBlock(int nbBlock)
        {
            defense += nbBlock;
            UpdateHealthBar();
        }

        //return the duration a the enemy hit visual effect
        public override float GetDamageHitDuration()
        {
            return enemyView.enemyHit.GetDuration();
        }
    }
}
