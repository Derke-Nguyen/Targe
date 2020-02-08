using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationFlags : MonoBehaviour
{
    // Animation Flags
    [SerializeField]
    private bool m_Hit = false;


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
