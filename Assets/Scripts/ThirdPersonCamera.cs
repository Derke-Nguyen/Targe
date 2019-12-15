using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private InputController input;

    // left and right of the camera
    private float m_Yaw;
    // the up and down of the camera
    private float m_Pitch;

    // max and min that the yaw can be
    private const float YAW_MAX = 30;
    private const float YAW_MIN = -30;

    // max and min that the pitch can be
    private const float PITCH_MAX = 45;
    private const float PITCH_MIN = -25;

    public float sensitivity = 1.5f;

    private Transform m_Target;
    private Transform m_PlayerPosition;
    private Transform m_Combat;
    private float m_DistanceFromTarget = 2;

    private bool m_LockOn = false;

    private void Start()
    {
        input = GameObject.Find("InputController").GetComponent<InputController>();
        m_Target = GameObject.Find("player").GetComponent<Transform>();
        m_Combat = GameObject.Find("CameraTarget").GetComponent<Transform>();
        m_PlayerPosition = GameObject.Find("player").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        if (m_Target == null)
            return;

        m_Yaw += input.CamHori() * sensitivity;
        if (m_LockOn)
        {
            m_Yaw = Mathf.Clamp(m_Yaw, YAW_MIN, YAW_MAX);
        }
        m_Pitch += input.CamVert() * sensitivity;
        m_Pitch = Mathf.Clamp(m_Pitch, PITCH_MIN, PITCH_MAX);

        Vector3 targetRotation = new Vector3(m_Pitch, m_Yaw);
        transform.eulerAngles = targetRotation;

        transform.position = m_Target.position - transform.forward * m_DistanceFromTarget;

    }

    public void LockOn()
    {
        m_LockOn = true;
        m_Target = m_Combat;
    }

    public void LockOff()
    {
        m_LockOn = false;
        m_Target = m_PlayerPosition;
    }
}
