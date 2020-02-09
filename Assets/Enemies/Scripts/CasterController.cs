using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterController : EnemyController
{

    public GameObject m_Fireball;
    private float m_ThrowStrength = 20f;

    public override void Start()
    {
        base.Start();
        m_DetectRange = 20f;
        m_AttackRange = 10f;
        m_MoveSpeed = 3f;
        m_TurnTime = 0.25f;
        m_HitSphereRange = 0.0f;
}

    public override void Combat()
    {
        base.Combat();
        if(m_AnimFlags.ThrowFireball())
        {
            //Create a fireball
            Vector3 direction = m_PlayerLocation.position - transform.position;
            direction.Normalize();
            GameObject thing = Instantiate(m_Fireball, m_Attackpoint.position, Quaternion.identity);
            thing.GetComponent<Rigidbody>().AddForce(direction * m_ThrowStrength, ForceMode.Impulse);
            m_AnimFlags.FireballThrown();
        }
    }
}
