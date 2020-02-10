using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyController : EnemyController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        m_AttackDamage = 15;
        m_Knockback = 1.2f;
        m_DetectRange = 10f;
        m_AttackRange = 1.75f;
        m_MoveSpeed = 3f;
        m_HitSphereRange = 0.4f;
    }

    public override void GotHit(int t_damage, bool m_unblockable)
    {
        if(m_unblockable)
        {
            SetState(State.hit);
        }
        m_Stats.Damage(t_damage);
    }
}
