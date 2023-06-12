using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamage;

    [SerializeField] private int health = 100;
    private int healthMax;

    private void Awake()
    {
        healthMax = health;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damageAmount"></param>
    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }

        OnDamage?.Invoke(this,EventArgs.Empty);

        if (health == 0)
        {
            Die();
        }

        Debug.Log(health);
    }
    /// <summary>
    /// 
    /// </summary>
    private void Die()
    {
        OnDead?.Invoke(this,EventArgs.Empty);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetHealthNormalized()
    {
        return (float)( health) / healthMax;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetCurrentHealth()
    {
        return health;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetCurrentHealthMax()
    {
        return healthMax;
    }

}
