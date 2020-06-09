using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    
    private Image health = null;

    float maxHealthPoint = 100;


    private void Start()
    {
        health = this.GetComponent<Image>();
    }

    /**
     * Called in Start of PlayerManager
     */
    public void SetMaxHealth(float maxHealth)
    {
        maxHealthPoint = maxHealth;
    }

    /**
     * update the health bar of player
     */
    public void UpdateHealthBar(float currentHealth)
    {
        health.fillAmount = currentHealth / maxHealthPoint;
    }
}
