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

    public float sensitivity = 0.8f;

    private Transform target;
    private float m_DistanceFromTarget = 2;

    private void Start()
    {
        input = GameObject.Find("InputController").GetComponent<InputController>();
        target = GameObject.Find("CameraTarget").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        m_Yaw += input.CamVert() * sensitivity;
        m_Yaw = Mathf.Clamp(m_Yaw, YAW_MIN, YAW_MAX);
        m_Pitch += input.CamHori() * sensitivity;
        m_Pitch = Mathf.Clamp(m_Pitch, PITCH_MIN, PITCH_MAX);

        Vector3 targetRotation = new Vector3(m_Yaw, m_Pitch);
        transform.eulerAngles = targetRotation;

        transform.position = target.position - transform.forward * m_DistanceFromTarget;

    }
}
