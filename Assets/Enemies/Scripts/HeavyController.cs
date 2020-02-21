/**
 * File: EnemyController.cs 
 * Author: Derek Nguyen
 * 
 * Child Class for EnemyController
 */
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyController : EnemyController
{
    /**
     * What happesn on start frame
     * 
     * Gathers all components that are needed and initializes the object
     * Sets stats specific for heavies
     */
    public override void Start()
    {
        base.Start();
        m_AttackDamage = 15;
        m_Knockback = 2f;
        m_DetectRange = 10f;
        m_AttackRange = 1.75f;
        m_MoveSpeed = 3f;
        m_HitSphereRange = 0.4f;
        m_Unblockable = true;
    }

    /**
     * When enemy is hit, do these operations
     * override for base GotHit
     * 
     * t_Damage : The damage that will be dealt
     * t_Stagger : If move will stagger or not
     */
    public override void GotHit(int t_Damage, bool t_Stagger = false)
    {
        m_Stats.Damage(t_Damage);
        if(t_Stagger)
        {
            SetState(State.hit);
        }
    }
}
