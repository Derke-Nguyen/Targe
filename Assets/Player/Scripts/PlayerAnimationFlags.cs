﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationFlags : MonoBehaviour
{
    // Animation Flags
    [SerializeField]
    private bool m_Dodge = false;
    private bool m_Combat = false;
    private bool m_CombatMove = false;
    private bool m_CombatHitBox = false;
    private bool m_Hit = false;


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

    public bool CombatStatus()
    {
        return m_Combat;
    }

    public void CombatStart()
    {
        m_Combat = false;
    }

    public void CombatEnded()
    {
        m_Combat = true;
    }

    private void ThrowShield()
    {
        ShieldController shield = GameObject.Find("shield").GetComponent<ShieldController>();
        Vector3 direction = Camera.main.transform.forward;
        Camera.main.transform.GetComponent<ThirdPersonCamera>().AimOff();
        shield.Thrown(direction);
    }

    public bool CombatMove()
    {
        return m_CombatMove;
    }

    public void CombatWindUpStart()
    {
        m_CombatMove = true;
    }

    public void CombatWindUpEnd()
    {
        m_CombatMove = false;
    }

    public bool CombatHitboxActive()
    {
        return m_CombatHitBox;
    }

    public void CombatHitboxStart()
    {
        m_CombatHitBox = true;
    }

    public void CombatHitboxEnd()
    {
        m_CombatHitBox = false;
    }

    public void ResetCombat()
    {
        m_Combat = false;
        m_CombatMove = false;
        m_CombatHitBox = false;
    }

    public bool HitStatus()
    {
        return m_Hit;
    }

    public void HitStart()
    {
        m_Hit = false;
    }

    public void HitEnded()
    {
        m_Hit = true;
    }
}
