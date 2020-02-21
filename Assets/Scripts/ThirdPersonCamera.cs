/**
 * File: ThirdPersonCamera.cs 
 * Author: Derek Nguyen
 * 
 * Controls the camera
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    // reference to the input controller
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

    //Distance of the camera to the target
    private float m_DistanceFromTarget = 2;

    // Default Distance camera should be from target
    private float m_DefaultDistance = 2;

    // Camera distance when aiming
    private float m_AimDistance = 2.5f;
    // Camera distance when aiming when locked on
    private float m_AimDistanceLO = 0.5f;

    // Bool if the camera is locked on
    private bool m_LockOn = false;

    // Bool if player is currently aiming shield
    private bool m_Aim = false;

    // If the camera is currently shaking
    private bool m_ScreenShake = false;

    /**
     * What happens on start frame
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

    /**
     * What happens every fixed amount of frames
     * 
     * If there is lock on, then set lockon at the correct position and rotation, unless the screen is shaking
     */
    private void FixedUpdate()
    {
        // If camera is locked on
        if (m_LockOn && !m_Aim && !m_ScreenShake)
        {
            Vector3 lookdir = m_Focus.position - transform.position;
            lookdir.Normalize();
            lookdir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookdir);
            transform.position = m_Target.position;
            m_Yaw = transform.eulerAngles.y;
            m_Pitch = transform.eulerAngles.x;
        }
    }

    /**
     *What happens every set amount of frames
     * 
     * Checks for any unexpected cases
     * Takes care of player if they're aiming or locked on
     */
    private void LateUpdate()
    {
        // if there isn't a target to lock on to
        if (m_Target == null)
            return;

        // if camera isn't looking at anything and you lock on, lock off
        if(m_Focus == null && m_LockOn)
        {
            LockOff();
        }

        // if camera is locked on and not aiming don't move camera
        if(m_LockOn && !m_Aim)
        {
            return;
        }

        // if screen is shaking, don't execute anything
        if (m_ScreenShake)
        {
            return;
        }

        // if aiming, move camera to aim accordingly
        if (m_Aim)
        {
            m_Yaw += input.CamHori() * aim_sensitivity;
            m_Pitch += input.CamVert() * aim_sensitivity;
        }
        else if(!m_LockOn)
        {
            m_Yaw += input.CamHori() * sensitivity;
            m_Pitch += input.CamVert() * sensitivity;
        }
        m_Pitch = Mathf.Clamp(m_Pitch, PITCH_MIN, PITCH_MAX);
        Vector3 targetRotation = new Vector3(m_Pitch, m_Yaw);

        // Set rotation and position
        transform.eulerAngles = targetRotation;
        transform.position = m_Target.position - transform.forward * m_DistanceFromTarget;
    }

    /**
     * Sets the camera to lock on
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

    /**
     * Sets the camera into its lock off settings
     * 
     * Sets the camera into its default position and sets lock on to false
     */
    public void LockOff()
    {
        m_LockOn = false;
        m_Target = m_PlayerPosition;
    }

    /**
     * Returns the target that is the lock on target
     *
     * return : target to lock on to
     */
    public Transform GetLockedOnTarget()
    {
        return m_Focus;
    }

    /**
     * Operations for when an object stays inside the collider of the camera
     * 
     * Finds an enemy and if the distance to the camera is less, then will set it as a lock on target
     */
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !m_LockOn)
        {
            //if already the focus don't bother
            if (other.gameObject.transform == m_Focus)
                return;
            //if there is no focus or current focus enemy is dead, set new focus
            if (m_Focus == null || m_Focus.gameObject.GetComponent<Stats>().IsDead())
            {
                m_Focus = other.gameObject.transform;
                return;
            }
            // Checks if new object is closer, set it to lock on target
            float DtoOg = Vector3.Distance(gameObject.transform.position, m_Focus.position);
            float DtoNew = Vector3.Distance(gameObject.transform.position, other.gameObject.transform.position);
            if (DtoOg > DtoNew)
            {
                m_Focus = other.gameObject.transform;
            }
        }
    }

    /**
     *S ets the camera into its aim settings
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

    /**
     * Sets the camera into its default settings
     * 
     * Sets the camera into its default position and sets aim to false
     */
    public void AimOff()
    {
        m_Aim = false;
        m_DistanceFromTarget = m_DefaultDistance;
    }

    /**
     * Shakes the camera
     * 
     * t_Duration : how long the camera will shake for
     * t_Magnitude : how hard the camera shakes
     */
    public IEnumerator Shake(float t_Duration, float t_Magnitude)
    {
        Vector3 originalPos = transform.position;
        m_ScreenShake = true;

        float elasped = 0.0f;
        while(elasped < t_Duration)
        {
            //Randomizes camera's new position
            float x = originalPos.x + Random.Range(-0.5f, 0.5f) * t_Magnitude;
            float y = originalPos.y + Random.Range(-0.5f, 0.5f) * t_Magnitude;
            float z = originalPos.z + Random.Range(-0.5f, 0.5f) * t_Magnitude;

            transform.position = new Vector3(x, y, z);
            
            elasped += Time.deltaTime;
            yield return null;
        }
        // resets camera to orignal position
        transform.position = originalPos;
        m_ScreenShake = false;
    }
}
