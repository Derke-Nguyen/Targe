/**
 * File: EnemyAnimationFlags.cs 
 * Author: Derek Nguyen
 * 
 * Manages flags for enemy animation
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationFlags : MonoBehaviour
{
    // Hit flag, false by default
    private bool m_Hit = false;
    // Combat flag, false by default
    private bool m_Combat = false;
    // Combat Hitbox flag, false by defualt
    private bool m_HitboxActive = false;
    // Fireball flag, false by defualt
    private bool m_Fireball = false;

    /**
     * Gets the status of hit animation
     * 
     * return : true if in hit animation, false otherwise
     */
    public bool HitStatus()
    {
        return m_Hit;
    }

    /**
     * Start of hit animation
     */
    public void HitStart()
    {
        m_Hit = true;
    }

    /**
     * End of combat animation
     */
    public void HitEnded()
    {
        m_Hit = false;
    }

    /**
 * Gets the status of combat animation
 * 
 * return : true if in combat animation, false otherwise
 */
    public bool CombatStatus()
    {
        return m_Combat;
    }

    /**
     * Start of combat animation
     */
    public void CombatStart()
    {
        m_Combat = true;
    }

    /**
     * End of combat animation
     */
    public void CombatEnded()
    {
        m_Combat = false;
    }

    /**
     * Gets the status if in hitbox frames
     * 
     * return : true if in hitbox frames, false otherwise
     */
    public bool CombatHitBox()
    {
        return m_HitboxActive;
    }

    /**
     * Start of combat hitbox frames
     */
    public void HitboxStart()
    {
        m_HitboxActive = true;
    }

    /**
     * End of combat hitbox frames
     */
    public void HitboxEnd()
    {
        m_HitboxActive = false;
    }

    /**
     * Gets the status if in throw fireball frames
     * 
     * return : true if in frames, false otherwise
     */
    public bool ThrowFireball()
    {
        return m_Fireball;
    }

    /**
     * Start of fireball throw frames
     */
    public void FireballCreate()
    {
        m_Fireball = true;
    }

    /**
     * End of fireball throw frames
     */
    public void FireballThrown()
    {
        m_Fireball = false;
    }
}
