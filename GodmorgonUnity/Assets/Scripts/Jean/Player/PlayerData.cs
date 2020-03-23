using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

public class PlayerData
{
    public int lifeMax;
    public int life;
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
        lifeMax = 100;
        power = 50;

        life = lifeMax;
    }

    public void SetLife(int newLife)
    {
        life = newLife;
    }

    public void SetPower(int newPower)
    {
        power = newPower;
    }
}
