using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Possible States the player can be in
    public enum State { idle, walking, dodge, hit, dead };

    // Player's current state
    private State m_State = State.idle;
    private State m_PrevState = State.idle;

    // Player's animator
    private Animator m_anim;

    // Input Controller
    private InputController input;

    // Camera
    private Camera m_Camera;

    // Stats
    private Stats m_stats;

    // Animation FLags
    private PlayerAnimationFlags animflags;

    // Movement
    private const float WALK_SPEED = 2;
    private const float RUN_SPEED = 6;
    private const float LOCK_SPEED = 1f;
    private const float TURN_TIME = 0.2f;
    private bool m_Run = false;
    private float m_TurnVel;
    private bool m_LockedOn;

    // Dodge
    private Vector3 m_DodgeDirection = Vector3.zero;
    private const float ROLL_SPEED = 8f;
    private const float JUKE_SPEED = 4f;

    // Shield
    [SerializeField]
    private bool m_Shield = false;

    private bool m_Dead = false;

    /*
     * What happens on start frame
     */
    private void Start()
    {
        input = GameObject.Find("InputController").GetComponent<InputController>();
        animflags = gameObject.GetComponentInChildren<PlayerAnimationFlags>();

        m_anim = gameObject.GetComponentInChildren<Animator>();
        m_Camera = Camera.main;
        m_stats = GetComponent<Stats>();
    }

    /*
     * What happens every frame
     */
    private void Update()
    {
        if(input.LockOn() && m_Camera.GetComponent<ThirdPersonCamera>().LockOn() && !m_LockedOn)
        {
            m_LockedOn = true;
            m_anim.SetBool("LockedOn", m_LockedOn);
        }
        else if(input.LockOn() && m_LockedOn)
        {
            m_LockedOn = false;
            m_anim.SetBool("LockedOn", m_LockedOn);
            m_Camera.GetComponent<ThirdPersonCamera>().LockOff();
        }

        if(m_stats.IsDead() && !m_Dead)
        {
            SetState(State.dead);
            m_Dead = true;
        }

        m_anim.SetBool("Shield", m_Shield);
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
        m_PrevState = m_State;
        m_State = t_state;
        
        switch (m_State)
        {
            case State.idle:
                m_Run = false;
                m_anim.SetFloat("MovementSpeed", 0);
                break;

            case State.walking:
                break;

            case State.dodge:
                Vector3 direction = new Vector3(input.Hori(), 0, input.Vert());
                if(direction == Vector3.zero)
                {
                    direction = gameObject.transform.forward;
                }
                m_DodgeDirection = direction;
                animflags.DodgeStart();
                break;

            case State.dead:
                m_anim.SetTrigger("Dead");
                break;
            default:
                break;
        }
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

            case State.dodge:
                Dodge();
                break;

            case State.dead:
                Dead();
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
        if (input.Dodge())
        {
            SetState(State.dodge);
        }
    }

    /*
     * Player actions for when they are walking/idle
     */
    private void Walking()
    {
        if (input.Vert() == 0 && input.Hori() == 0)
        {
            SetState(State.idle);
        }
        if (input.Dodge())
        {
            SetState(State.dodge);
        }

        if (m_LockedOn)
        {
            LockedOn();

            //Forward and backward movement
            float fbspeed = LOCK_SPEED * input.Vert();
            transform.Translate(transform.forward * fbspeed * Time.deltaTime, Space.World);

            //Left and right movement
            float lrspeed = LOCK_SPEED * input.Hori();
            transform.Translate(transform.right * lrspeed * Time.deltaTime, Space.World);

            m_anim.SetFloat("LockedOnHori", input.Hori());
            m_anim.SetFloat("LockedOnVert", input.Vert());
        }

        else
        {
            Vector2 inputDir = new Vector2(input.Hori(), input.Vert());

            float targetRot = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + m_Camera.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref m_TurnVel, TURN_TIME);

            if (input.Run())
            {
                m_Run = !m_Run;
            }
            float speed = (m_Run ? RUN_SPEED : WALK_SPEED) * inputDir.magnitude;
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            m_anim.SetFloat("MovementSpeed", speed / 6);
        }


    }

    /* What the player does when the player is in the dash state
     */ 
    private void Dodge()
    {
        if(m_LockedOn)
        {
            LockedOn();

            //Left and right movement
            float lrspeed = JUKE_SPEED * m_DodgeDirection.x;
            transform.Translate(transform.right * lrspeed * Time.deltaTime, Space.World);

            //Forward and backward movement
            float fbspeed = JUKE_SPEED * m_DodgeDirection.z;
            transform.Translate(transform.forward * fbspeed * Time.deltaTime, Space.World);

            m_anim.SetBool("Dodge", true);
            m_anim.SetFloat("LockedOnHori", m_DodgeDirection.x);
            m_anim.SetFloat("LockedOnVert", m_DodgeDirection.z);
        }
        else
        {
            float targetRot = Mathf.Atan2(m_DodgeDirection.x, m_DodgeDirection.z) * Mathf.Rad2Deg + m_Camera.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref m_TurnVel, TURN_TIME);
            float speed = ROLL_SPEED * m_DodgeDirection.magnitude;
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            m_anim.SetBool("Dodge", true);
        }

        if(animflags.DodgeStatus())
        {
            m_anim.SetBool("Dodge", false);
            SetState(m_PrevState);
        }
    }

    /* When the lockon button is pressed, how it affects the player
     */ 
    private void LockedOn()
    {
        Transform thing = m_Camera.GetComponent<ThirdPersonCamera>().GetLockedOnTarget();
        if(thing == null || thing.gameObject.GetComponent<Stats>().IsDead())
        {
            m_LockedOn = false;
            m_anim.SetBool("LockedOn", m_LockedOn);
            m_Camera.GetComponent<ThirdPersonCamera>().LockOff();
        }
        Vector3 thingDir = new Vector3(thing.position.x, transform.position.y, thing.position.z);
        transform.LookAt(thingDir);
        m_anim.SetBool("LockedOn", m_LockedOn);
    }

    private void Dead()
    {

    }
}
