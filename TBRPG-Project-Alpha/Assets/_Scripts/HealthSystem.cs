using System;
using UnityEngine;
/// <summary>
/// The health system.
/// </summary>

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamage;

    [SerializeField]private int health = 30;
    private int healthMax;

    //private void Start()
    //{
    //    healthMax = health;
    //}
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

        OnDamage?.Invoke(this, EventArgs.Empty);

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
        OnDead?.Invoke(this, EventArgs.Empty);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetHealthNormalized()
    {
        return (float)(health) / healthMax;
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

    /// <summary>
    /// Sets the health.
    /// </summary>
    /// <param name="health">The health.</param>
    public void SetHealth(int health)
    {
        this.health = health;
        this.healthMax = health;
    }

}
