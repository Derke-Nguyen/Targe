using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private int m_maxhealth = 100;
    [SerializeField, Range(0.0f, 100.0f)]
    private int m_currhealth = 100;
    [SerializeField]
    private bool m_dead = false;
    private bool m_invincible = false;

    public void SetHealth(int t_health)
    {
        if(t_health < 0)
        {
            Debug.Log("Invalid Health: " + t_health);
            return;
        }
        m_currhealth = t_health;
        if(m_currhealth > m_maxhealth)
        {
            m_currhealth = m_maxhealth;
        }
    }

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

    public int GetHealth()
    {
        return m_currhealth;
    }

    public int GetMaxHealth()
    {
        return m_maxhealth;
    }

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

    public void Heal()
    {
        m_currhealth = m_maxhealth;
    }

    public bool IsDead()
    {
        return m_dead;
    }
    
    public void EnableIFrame()
    {
        m_invincible = true;
    }

    public void DisableIFrame()
    {
        m_invincible = false;
    }
}
