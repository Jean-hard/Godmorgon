using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.Player
{
    public class PlayerData : Entity
    {
        public int healthMax;
        public int defenseMax;

        public int health;
        public int defense;
        public int power;

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
            power = 50;

            health = healthMax;
        }

        public void SetHealth(int newHealth)
        {
            health = newHealth;
        }

        public void SetPower(int newPower)
        {
            power = newPower;
        }

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

        public void AddBlock(int blockValue)
        {
            defense += blockValue;
            if (defense > defenseMax)
                defense = defenseMax;
        }
    }
}
