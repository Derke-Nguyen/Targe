using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Maximum Health of this Entity
    private int m_maxhealth = 100;
    
    // Current Health of this Entity
    private int m_currhealth = 100;
    
    // If this entity is current dead
    [SerializeField]private bool m_dead = false;

    // If this entity is invincible
    private bool m_invincible = false;

    /* Sets the current health of the entity
     * 
     * t_health : int that the current health will be set at
     */
    public void SetCurrHealth(int t_health)
    {
        if(t_health < 0)
        {
            Debug.Log("Invalid Health: " + t_health);
            return;
        }
        m_currhealth = t_health;
        if (m_currhealth <= 0)
        {
            m_dead = true;
            m_currhealth = 0;
        }
        if (m_currhealth > m_maxhealth)
        {
            m_currhealth = m_maxhealth;
        }
    }

    /* Sets the max health of the entity
     * 
     * t_health : int that the max health will be set at
     */
    public void SetMaxHealth(int t_health)
    {
        if (t_health < 0)
        {
            Debug.Log("Invalid Health: " + t_health);
            return;
        }
        m_maxhealth = t_health;
        if (m_currhealth > m_maxhealth)
        {
            m_currhealth = m_maxhealth;
        }
    }

    /* Returns the entity's current health
     * 
     * returns : current entity's health
     */
    public int GetHealth()
    {
        return m_currhealth;
    }

    /* Returns the entity's max health
     * 
     * returns : entity's max health
     */
    public int GetMaxHealth()
    {
        return m_maxhealth;
    }

    /* Damages the entity
    * 
    * t_damage : damage that will be dealt to enemy
    */
    public void Damage(int t_damage)
    {
        if (m_invincible)
            return;

        m_currhealth -= t_damage;
        if(m_currhealth <= 0)
        {
            m_dead = true;
            m_currhealth = 0;
        }
    }

    /* Heals the entity
     * 
     * t_health : amount that current health will be increased by
     */
    public void Heal(int t_health)
    {
        m_currhealth += t_health;
        if (m_currhealth > m_maxhealth)
        {
            m_currhealth = m_maxhealth;
        }
    }

    /* Tells other scripts if entity is dead
     * 
     * return : if the entity is dead
     */
    public bool IsDead()
    {
        return m_dead;
    }

    /* Sets if the entity has an iframe
     * 
     * sets iframe to true
     */
    public void EnableIFrame()
    {
        m_invincible = true;
    }

    /* Disable the entity's iframe 
     * 
     * sets iframe to false
     */
    public void DisableIFrame()
    {
        m_invincible = false;
    }
}
