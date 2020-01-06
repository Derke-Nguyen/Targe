using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationFlags : MonoBehaviour
{
    // Animation Flags
    [SerializeField]
    private bool m_Dodge = false;


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

    private void ThrowShield()
    {
        ShieldController shield = GameObject.Find("shield").GetComponent<ShieldController>();
        Vector3 direction = Camera.main.transform.forward;
        Camera.main.transform.GetComponent<ThirdPersonCamera>().AimOff();
        shield.Thrown(direction);
    }
    
}
