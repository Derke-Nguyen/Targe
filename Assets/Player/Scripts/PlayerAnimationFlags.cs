using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationFlags : MonoBehaviour
{
    // Animation Flags
    [SerializeField]
    private bool m_Dodge = false;
    private bool m_Combat = false;
    private bool m_CombatWindup = false;


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
        return m_Dodge;
    }

    public void CombatStart()
    {
        m_Dodge = false;
    }

    public void CombatEnded()
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

    private void PushForward(float t_Speed)
    {
        transform.parent.transform.Translate(transform.parent.transform.forward * t_Speed * Time.deltaTime, Space.World);
    }

    public bool CombatWindUp()
    {
        return m_CombatWindup;
    }

    public void CombatWindUpStart()
    {
        m_CombatWindup = true;
    }

    public void CombatWindUpEnd()
    {
        m_CombatWindup = false;
    }
    
}
