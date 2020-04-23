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

        //--------------- player data in game ---------------------
        public int health;
        public int defense;

        
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

            health = healthMax;
        }

        public void SetHealth(int newHealth)
        {
            health = newHealth;
        }

        /**
         * Update health and defense player data
         * must be called from player manager !!
         */
        public override void TakeDamage(int damagePoint)
        {
            Debug.Log("player health before was : " + health);
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
            Debug.Log("player health after was : " + health);
        }

        /**
         * add block to player data
         * must be called from player manager !!
         */
        public void AddBlock(int blockValue)
        {
            defense += blockValue;
            if (defense > defenseMax)
                defense = defenseMax;
        }

        //Set the damage done and taken for the killer instinct effect
        public void OnKillerInstinct()
        {
            doubleDamageDone = true;
            doubleDamageTaken = true;
        }
    }
}
