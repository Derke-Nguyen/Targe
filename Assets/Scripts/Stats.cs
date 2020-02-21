/**
 * File: Stats.cs 
 * Author: Derek Nguyen
 * 
 * Class that holds all health and statuses
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Maximum Health of this Entity
    [SerializeField]
    private int m_MaxHealth = 100;
    
    // Current Health of this Entity
    private int m_CurrHealth = 100;
    
    // If this entity is current dead
    [SerializeField]private bool m_Dead = false;

    /** 
     * What happesn on start frame
     * 
     * Sets current health as max health
     */
    private void Start()
    {
        m_CurrHealth = m_MaxHealth;
    }

    /**
     * Sets the current health of the entity
     * 
     * t_Health : int that the current health will be set at
     */
    public void SetCurrHealth(int t_Health)
    {
        if(t_Health < 0)
        {
            Debug.Log("Invalid Health: " + t_Health);
            return;
        }
        m_CurrHealth = t_Health;
        if (m_CurrHealth <= 0)
        {
            m_Dead = true;
            m_CurrHealth = 0;
        }
        if (m_CurrHealth > m_MaxHealth)
        {
            m_CurrHealth = m_MaxHealth;
        }
    }

    /**
     * Sets the max health of the entity
     * 
     * t_Health : int that the max health will be set at
     */
    public void SetMaxHealth(int t_Health)
    {
        if (t_Health < 0)
        {
            Debug.Log("Invalid Health: " + t_Health);
            return;
        }
        m_MaxHealth = t_Health;
        if (m_CurrHealth > m_MaxHealth)
        {
            m_CurrHealth = m_MaxHealth;
        }
    }

    /**
     * Returns the entity's current health
     * 
     * returns : current entity's health
     */
    public int GetHealth()
    {
        return m_CurrHealth;
    }

    /**
     * Returns the entity's max health
     * 
     * returns : entity's max health
     */
    public int GetMaxHealth()
    {
        return m_MaxHealth;
    }

    /**
     * Returns the entity's percent health
     * 
     * returns : entity's percent health
     */
    public float GetPercentHealth()
    {
        return ((float)m_CurrHealth / (float)m_MaxHealth);
    }

    /**
     * Damages the entity
     * 
     * t_Damage : damage that will be dealt to enemy
     */
    public void Damage(int t_Damage)
    {
        m_CurrHealth -= t_Damage;
        if(m_CurrHealth <= 0)
        {
            m_Dead = true;
            m_CurrHealth = 0;
        }
    }

    /**
     * Heals the entity
     * 
     * t_Health : amount that current health will be increased by
     */
    public void Heal(int t_Health)
    {
        m_CurrHealth += t_Health;
        if (m_CurrHealth > m_MaxHealth)
        {
            m_CurrHealth = m_MaxHealth;
        }
    }

    /**
     * Tells other scripts if entity is dead
     * 
     * return : if the entity is dead
     */
    public bool IsDead()
    {
        return m_Dead;
    }
}
