using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.Player
{
    public class PlayerData
    {
        public int healthMax;
        public int health;
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
    }
}
