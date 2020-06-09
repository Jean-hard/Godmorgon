using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.Player
{
    public class PlayerData : Entity
    {
        //set at start
        public int healthMax;
        public int defenseMax;
        public int startGold;
        public int startToken;

        //--------------- player data in game ---------------------
        public int health;
        public int defense;
        public int goldValue;
        public int token;
        
        public bool doubleDamageDone = false;
        public bool doubleDamageTaken = false;

        //--------------------------------------------------------

        #region Singleton Pattern
        private static PlayerData instance;

        public static PlayerData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerData();
                }

                return instance;
            }
        }
        #endregion

        //Sera créé et configuré par le gameEngine
        public PlayerData()
        {
            //à configurer par le gameEngine
            healthMax = 100;
            defenseMax = 100;
            startGold = 80;
            startToken = 3;

            health = healthMax;
            goldValue = startGold;
            token = startToken;
        }

        public void SetHealth(int newHealth)
        {
            health = newHealth;
        }

        /**
         * Update health and defense player data
         * must be called from player manager !!
         */
        public override void TakeDamage(int damagePoint, bool isPlayerAttacking)
        {
            //si killerInstinct est activé
            if (doubleDamageTaken)
            {
                damagePoint = damagePoint * 2;
                //Debug.Log("damage reçu doublé !");
            }
            //Debug.Log("player health before was : " + health);
            while (damagePoint > 0 && defense > 0)
            {
                defense--;
                damagePoint--;
            }

            while (damagePoint > 0 && health > 0)
            {
                health--;
                damagePoint--;
            }
            //Debug.Log("player health after was : " + health);

            PlayerManager.Instance.UpdateHealthBar(health + defense);
        }

        /**
         * retourne les dégats correspondant au bonus de stat du player
         */
        public override int DoDamage(int baseDamagePoint)
        {
            if(doubleDamageDone)
            {
                //Debug.Log("damage donné double !");
                baseDamagePoint = baseDamagePoint * 2;
            }
            return baseDamagePoint;
        }

        /**
         * Retourne true si le joueur n'a plus de vie
         */
        public override bool IsDead()
        {
            if (health <= 0) return true;

            return false;
        }

        /**
         * add block to player data
         * must be called from player manager !!
         */
        public void AddBlock(int blockValue)
        {
            health += blockValue;
            if (health > healthMax)
                health = healthMax;
        }

        //Set the damage done and taken for the killer instinct effect
        public void OnKillerInstinct()
        {
            doubleDamageDone = true;
            doubleDamageTaken = true;
        }

        //réinitialise toutes stats du player
        public void ResetStat()
        {
            //on retire killer instinct
            doubleDamageDone = false;
            doubleDamageTaken = false;
        }

        //retourne vrai si la santé actuel du player est à la moitié ou moins
        public bool IsHealthAtHalf()
        {
            if (health <= healthMax / 2)
                return true;
            else
                return false;
        }

        //Ajoute des golds au player
        public void AddGold(int value)
        {
            goldValue += value;
        }

        //Dépense de l'argent
        public void SpendGold(int value)
        {
            goldValue -= value;
        }

        //Ajoute un token au player
        public void AddToken()
        {
            token++;
        }

        //Enlève 1 token
        public void TakeOffOneToken()
        {
            token--;
        }

        //Retourne true si le joueur peut dépenser tant d'argent
        public bool HasEnoughGold(int value)
        {
            if (goldValue < value) return false;
            
            return true;
        }
    }
}
