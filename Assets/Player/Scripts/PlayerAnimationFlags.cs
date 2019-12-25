using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationFlags : MonoBehaviour
{
    // Animation Flags
    [SerializeField]
    private bool m_Rolled = false;

    public bool RollStatus()
    {
        return m_Rolled;
    }

    public void RollStart()
    {
        m_Rolled = false;
    }

    public void RollEnded()
    {
        m_Rolled = true;
    }
    
}
