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

    // Sensitivity of the camera movement
    public float sensitivity = 2.0f;
    public float aim_sensitivity = 1.25f;

    // Location of the Camera
    private Transform m_Target;
    // The center of the player
    private Transform m_PlayerPosition;
    // Over the shoulder camera position
    private Transform m_Combat;
    // Enemy target locked on to
    private Transform m_Focus;

    // Camera Distance from target
    private float m_DefaultDistance = 2;
    private float m_DistanceFromTarget = 2;

    private bool m_LockOn = false;

    private bool m_Aim = false;

    // Camera distance when aiming
    private float m_AimDistance = 2.5f;
    private float m_AimDistanceLO = 0.25f;

    /* What happesn on start frame
    * 
    * Gathers all components that are needed and initializes the object
    */
    private void Start()
    {
        input = GameObject.Find("InputController").GetComponent<InputController>();
        m_PlayerPosition = GameObject.Find("player").GetComponent<Transform>();
        m_Target = m_PlayerPosition;
        m_Combat = GameObject.Find("CameraTarget").GetComponent<Transform>();
    }

    /* What happens every fixed amount of frames
     * 
     * If there is lock on, then set lockon at the correct position and rotation
     */
    private void FixedUpdate()
    {
        // If camera is locked on
        if (m_LockOn)
        {
            Vector3 lookdir = m_Focus.position - transform.position;
            lookdir.Normalize();
            lookdir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookdir);
            transform.position = m_Target.position;
        }
    }

    /* What happens every set amount of frames
    * 
    * Checks for any unexpected cases
    * Takes care of player if they're aiming or locked on
    */
    private void LateUpdate()
    {
        if (m_Target == null)
            return;

        if(m_Focus == null && m_LockOn)
        {
            m_LockOn = false;
            m_Target = m_PlayerPosition;
        }

        if(m_LockOn && !m_Aim)
        {
            return;
        }

        Vector3 targetRotation = new Vector3();

        if(m_Aim)
        {
            m_Yaw += input.CamHori() * aim_sensitivity;
            m_Pitch += input.CamVert() * aim_sensitivity;
            m_Pitch = Mathf.Clamp(m_Pitch, PITCH_MIN, PITCH_MAX);
            if (m_LockOn)
            {
                m_Yaw = Mathf.Clamp(m_Yaw, YAW_MIN, YAW_MAX);
            }
            targetRotation = new Vector3(m_Pitch, m_Yaw);
        }

        else if(!m_LockOn)
        {
            m_Yaw += input.CamHori() * sensitivity;
            m_Pitch += input.CamVert() * sensitivity;
            m_Pitch = Mathf.Clamp(m_Pitch, PITCH_MIN, PITCH_MAX);
            targetRotation = new Vector3(m_Pitch, m_Yaw);
        }

        // Set rotation and position
        transform.eulerAngles = targetRotation;
        transform.position = m_Target.position - transform.forward * m_DistanceFromTarget;
    }

    /* Sets the camera into its lock on settings
     * 
     * Sets the camera into its lock on position and sets lock on bool to true
     */ 
    public bool LockOn()
    {
        if(m_Focus == null || m_Focus.gameObject.GetComponent<Stats>().IsDead())
        {
            return false;
        }
        m_LockOn = true;
        m_Target = m_Combat;
        return true;
    }

    /* Sets the camera into its lock off settings
     * 
     * Sets the camera into its default position and sets lock on to false
     */
    public void LockOff()
    {
        m_LockOn = false;
        m_Target = m_PlayerPosition;
    }

    /* Returns the target that is the lock on target
     *
     * return : target to lock on to
     */
    public Transform GetLockedOnTarget()
    {
        return m_Focus;
    }

    /* Operations for when an object stays inside the collider of the camera
     * 
     * Finds an enemy and if the distance to the camera is less, then will set it as a lock on target
     */
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !m_LockOn)
        {
            if (other.gameObject.transform == m_Focus)
                return;
            if (m_Focus == null || m_Focus.gameObject.GetComponent<Stats>().IsDead())
            {
                m_Focus = other.gameObject.transform;
                return;
            }
            float DtoOg = Vector3.Distance(gameObject.transform.position, m_Focus.position);
            float DtoNew = Vector3.Distance(gameObject.transform.position, other.gameObject.transform.position);

            if (DtoOg > DtoNew)
            {
                m_Focus = other.gameObject.transform;
            }
        }
    }

    /* Sets the camera into its aim settings
     * 
     * Sets the camera into its aim position and sets aim bool to true
     */
    public void AimOn()
    {
        m_Aim = true;
        if(m_LockOn)
        {
            m_DistanceFromTarget = m_AimDistanceLO;
        }
        else
        {
            m_DistanceFromTarget = m_AimDistance;
        }
    }

    /* Sets the camera into its default settings
     * 
     * Sets the camera into its default position and sets aim to false
     */
    public void AimOff()
    {
        m_Aim = false;
        m_DistanceFromTarget = m_DefaultDistance;
    }
}
