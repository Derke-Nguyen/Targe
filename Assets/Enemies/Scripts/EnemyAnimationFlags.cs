using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationFlags : MonoBehaviour
{
    // Animation Flags
    [SerializeField]
    private bool m_Hit = false;
    private bool m_Combat = false;
    private bool m_HitboxActive = false;

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

    public bool CombatStatus()
    {
        return m_Combat;
    }

    public void CombatStart()
    {
        m_Combat = true;
    }

    public void CombatEnded()
    {
        m_Combat = false;
    }

    public bool CombatHitBox()
    {
        return m_HitboxActive;
    }

    public void HitboxStart()
    {
        m_HitboxActive = true;
    }

    public void HitboxEnd()
    {
        m_HitboxActive = false;
    }
}
