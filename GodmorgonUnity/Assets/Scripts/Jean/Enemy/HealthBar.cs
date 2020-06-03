using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Enemy;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Transform defense = null;
    [SerializeField]
    private Transform health = null;

    private float maxHealthPoint = 1f;
    private float maxDefensePoint = 1f;

    /**
     * call at start of enemyView
     * Set the maxHealth and maxdefense point
     */
    public void SetBarPoints(float maxHealth, float maxDefense)
    {
        maxDefensePoint = maxDefense;
        maxHealthPoint = maxHealth;
    }

    /**
     * update the health and defense bar
     */
    public void UpdateHealthBar(float currentDefense, float currentHealth)
    {
        if (maxDefensePoint < currentDefense)
            maxDefensePoint = currentDefense;

        if(currentDefense == 0)
            defense.localScale = new Vector2(0, defense.localScale.y);
        else
            defense.localScale = new Vector2((currentDefense / maxDefensePoint), defense.localScale.y);

        if(currentHealth == 0)
            health.localScale = new Vector2(0, defense.localScale.y);
        else
            health.localScale = new Vector2((currentHealth / maxHealthPoint), defense.localScale.y);
    }

    public void SetHealth(float newHealthValue)
    {
        defense.localScale = new Vector3(newHealthValue * 0.01f, 1f);
    }
}
