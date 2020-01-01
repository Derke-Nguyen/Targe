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
    private const float YAW_MAX = 20;
    private const float YAW_MIN = -20;

    // max and min that the pitch can be
    private const float PITCH_MAX = 45;
    private const float PITCH_MIN = -25;

    public float sensitivity = 3.0f;

    // Location of the Camera
    private Transform m_Target;
    // The center of the player
    private Transform m_PlayerPosition;
    // Over the shoulder camera position
    private Transform m_Combat;
    // Enemy target locked on to
    private Transform m_Focus;
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

        if(m_Focus == null && m_LockOn)
        {
            m_LockOn = false;
            m_Target = m_PlayerPosition;
        }

        // If camera is locked on
        if (m_LockOn)
        {
            transform.LookAt(m_Focus);
            transform.position = m_Target.position;
        }
        else
        {
            m_Yaw += input.CamHori() * sensitivity;
            m_Pitch += input.CamVert() * sensitivity;
            m_Pitch = Mathf.Clamp(m_Pitch, PITCH_MIN, PITCH_MAX);
            Vector3 targetRotation = new Vector3(m_Pitch, m_Yaw);

            transform.eulerAngles = targetRotation;

            transform.position = m_Target.position - transform.forward * m_DistanceFromTarget;
        }
    }

    public bool LockOn()
    {
        if(m_Focus == null)
        {
            return false;
        }
        m_LockOn = true;
        m_Target = m_Combat;
        return true;
    }

    public Transform GetLockedOnTarget()
    {
        return m_Focus;
    }

    public void LockOff()
    {
        m_LockOn = false;
        m_Target = m_PlayerPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && !m_LockOn)
        {
            if(m_Focus == null)
            {
                m_Focus = other.gameObject.transform;
                return;
            }
            if (other.gameObject.transform == m_Focus)
                return;

            float DtoOg = Vector3.Distance(gameObject.transform.position, m_Focus.position);
            float DtoNew = Vector3.Distance(gameObject.transform.position, other.gameObject.transform.position);

            if(DtoOg > DtoNew)
            {
                m_Focus = other.gameObject.transform;
            }
        }
    }
}
