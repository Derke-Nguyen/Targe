using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Possible States the player can be in
    public enum State { idle, walking };

    // Player's current state
    [SerializeField]
    private State m_State = State.idle;

    // Player's animator
    private Animator anim;

    // Input Controller
    private InputController input;

    // Camera
    private Camera m_Camera;

    // Movement
    private float WALK_SPEED = 2;
    private float RUN_SPEED = 6;
    private bool m_Run = false;
    private const float TURN_TIME = 0.2f;
    private float m_TurnVel;
    [SerializeField]
    private bool m_LockedOn;
    /*
     * What happens on start frame
     */
    private void Start()
    {
        anim = this.GetComponentInChildren<Animator>();
        input = GameObject.Find("InputController").GetComponent<InputController>();
        m_Camera = Camera.main;
    }

    /*
     * What happens every frame
     */
    private void Update()
    {
        if(input.LockOn() && m_Camera.GetComponent<ThirdPersonCamera>().LockOn())
        {
            m_LockedOn = true;
            anim.SetBool("LockedOn", m_LockedOn);
        }
    }

    /*
     * What happens every fixed amount of frames
     */
    private void FixedUpdate()
    {
        ExecuteState();
    }

    /*
     * Sets the player's state
     * t_state : the state that they player will be set as
     */
    private void SetState(State t_state)
    {
        m_State = t_state;
        m_Run = false;
    }

    /*
     * Executes player instructions based on what state the player is in
     */
    private void ExecuteState()
    {
        switch(m_State)
        {
            case State.idle:
                Idle();
                break;

            case State.walking:
                Walking();
                break;

            default:
                break;
        }
    }

    /*
     * Player actions for when they are idle
     */ 
    private void Idle()
    {
        if (m_LockedOn)
        {
            LockedOn();
        }
        if (input.Vert() != 0 || input.Hori() != 0)
        {
            SetState(State.walking);
        }
    }

    /*
     * Player actions for when they are walking/idle
     */
    private void Walking()
    {
        if(m_LockedOn)
        {
            LockedOn();

            Vector2 inputDir = new Vector2(input.Hori(), input.Vert());

            if (inputDir != Vector2.zero)
            {
                float targetRot = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + m_Camera.transform.eulerAngles.y;
                //transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref m_TurnVel, TURN_TIME);
            }

            float speed = WALK_SPEED * inputDir.magnitude;
            //transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            anim.SetFloat("MovementSpeed", speed / 6);
            anim.SetFloat("LockedOnHori", input.Hori());
            anim.SetFloat("LockedOnVert", input.Vert());
        }

        else
        {
            Vector2 inputDir = new Vector2(input.Hori(), input.Vert());

            if (inputDir != Vector2.zero)
            {
                float targetRot = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + m_Camera.transform.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref m_TurnVel, TURN_TIME);

            }

            if (input.Run())
            {
                m_Run = true;
            }
            float speed = (m_Run ? RUN_SPEED : WALK_SPEED) * inputDir.magnitude;
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            anim.SetFloat("MovementSpeed", speed / 6);
        }

        if (input.Vert() == 0 && input.Hori() == 0)
        {
            SetState(State.idle);
        }
    }

    private void LockedOn()
    {
        Transform thing = m_Camera.GetComponent<ThirdPersonCamera>().GetLockedOnTarget();
        Vector3 thingDir = new Vector3(thing.position.x, transform.position.y, thing.position.z);
        transform.LookAt(thingDir);
        anim.SetBool("LockedOn", m_LockedOn);
    }
}
