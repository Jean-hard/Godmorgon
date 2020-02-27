using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : MonoBehaviour
{
    public int lifeMax = 100;
    public int life;
    public int power = 50;
    
    private static PlayerMgr instance;

    public static PlayerMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayerMgr>();

                if (instance == null)
                {
                    GameObject container = new GameObject("Player");
                    instance = container.AddComponent<PlayerMgr>();
                }
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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

    public void AddStuffCardToInventory(StuffCard stuffCardToAdd)
    {
        
    }
}
