/**
 * File: PlayerAnimationFlags.cs 
 * Author: Derek Nguyen
 * 
 * Manages flags for player animation
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationFlags : MonoBehaviour
{
    // Dodge flag, false by default
    private bool m_Dodge = false;
    // Combat flag, true by default
    private bool m_Combat = false;
    // Combat Movement flag, false by defualt
    private bool m_CombatMove = false;
    // Combat Hitbox flag, false by default
    private bool m_CombatHitBox = false;
    // Hit flag, false by default
    private bool m_Hit = false;

    /**
     * Gets the status of dodge animation
     * 
     * return : true if in dodge animation, false otherwise
     */ 
    public bool DodgeStatus()
    {
        return m_Dodge;
    }

    public void DodgeStart()
    {
        m_Dodge = false;
    }

    public void DodgeEnded()
    {
        m_Dodge = true;
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
     * For specific frame where player will throw the shield 
     */
    private void ThrowShield()
    {
        ShieldController shield = GameObject.Find("shield").GetComponent<ShieldController>();
        Vector3 direction = Camera.main.transform.forward;
        Camera.main.transform.GetComponent<ThirdPersonCamera>().AimOff();
        shield.Thrown(direction.normalized);
    }

    /**
     * Gets the status if in attack move frames
     * 
     * return : true if in move frames, false otherwise
     */
    public bool CombatMove()
    {
        return m_CombatMove;
    }

    /**
     * Start of combat move frames
     */
    public void CombatWindUpStart()
    {
        m_CombatMove = true;
    }

    /**
     * End of combat move frames
     */
    public void CombatWindUpEnd()
    {
        m_CombatMove = false;
    }

    /**
     * Gets the status if in hitbox frames
     * 
     * return : true if in hitbox frames, false otherwise
     */
    public bool CombatHitboxActive()
    {
        return m_CombatHitBox;
    }

    /**
     * Start of combat hitbox frames
     */
    public void CombatHitboxStart()
    {
        m_CombatHitBox = true;
    }

    /**
     * End of combat hitbox frames
     */
    public void CombatHitboxEnd()
    {
        m_CombatHitBox = false;
    }

    /**
     * Resetting Combat animations
     */
    public void ResetCombat()
    {
        m_Combat = false;
        m_CombatMove = false;
        m_CombatHitBox = false;
    }

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
     * Resets the status of all the flags
     * Mostly for cancel animations
     */ 
    public void ResetFlags()
    {
        m_Dodge = false;
        m_Combat = false;
        m_CombatMove = false;
        m_CombatHitBox = false;
        m_Hit = false;
    }
}
