﻿/**
 * File: PlayerController.cs 
 * Author: Derek Nguyen
 * 
 * Child class for a MenuButton
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Possible States the player can be in
    public enum State { idle, walking, dodge, hit, dead, throwing, recall , combat, block};

    // Player's current state
    [SerializeField]
    private State m_State = State.idle;
    // Player's previous state
    private State m_PrevState = State.idle;

    // Player's animator
    private Animator m_Anim;

    // Input Controller
    private InputController input;

    // Camera
    private Camera m_Camera;

    // Stats
    private Stats m_Stats;

    // Animation FLags
    private PlayerAnimationFlags m_AnimFlags;

    #region Movement
    private const float WALK_SPEED = 2;
    private const float RUN_SPEED = 6;
    private const float LOCK_SPEED = 2f;
    private const float TURN_TIME = 0.4f;
    private bool m_Run = false;
    private float m_TurnVel;
    private bool m_LockedOn;
    #endregion

    #region Dodge
    private Vector3 m_DodgeDirection = Vector3.zero;
    // player's roll speed
    private const float ROLL_SPEED = 8f;
    // player's dodge speed when locked on
    private const float JUKE_SPEED = 6f;
    #endregion

    #region Shield
    [SerializeField]
    private bool m_Shield = false;
    private ShieldController m_ShieldController;
    #endregion

    // Dead
    private bool m_Dead = false;

    #region Combat
    private float m_TimeBetweenCombo = 0f;
    private const float DELAY_BETWEEN_COMBO = 0.05f;
    private bool m_CombatBuffered = false;
    [SerializeField]
    private Transform m_LeftFist;
    private Transform m_RightFist;
    private Transform m_Foot;
    private Transform m_ShieldPoint;
    private float ATTACK_RANGE = 0.2f;
    private float SHIELD_RANGE = 0.45f;
    public LayerMask m_EnemyLayer;

    private int FIST_DAMAGE = 10;
    private float FIST_KNOCKBACK = 0.25f;
    private int SHIELD_DAMAGE = 20;
    private float SHIELD_KNOCKBACK = 0.5f;

    private Dictionary<string, GameObject> m_AlreadyHit = new Dictionary<string, GameObject>();
    #endregion

    private bool m_Paused = false;

    /**
     *What happens on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    private void Start()
    {
        input = GameObject.Find("InputController").GetComponent<InputController>();
        m_AnimFlags = gameObject.GetComponentInChildren<PlayerAnimationFlags>();
        m_Anim = gameObject.GetComponentInChildren<Animator>();
        m_Camera = Camera.main;
        m_Stats = GetComponent<Stats>();
        m_ShieldController = GameObject.Find("shield").GetComponent<ShieldController>();
        m_LeftFist = GameObject.Find("LeftFist").transform;
        m_RightFist = GameObject.Find("RightFist").transform;
        m_Foot = GameObject.Find("LeftFoot").transform;
        m_ShieldPoint = GameObject.Find("ShieldPoint").transform;
    }

    /* What happens every frame
     * 
     * Checks for certain inputs that will override all states
     * Also tells animator if there is a shield equipped
     * Also checks if player is dead
     */
    private void Update()
    {
        if(m_Paused)
        {
            return;
        }
        //No character control if player is already dead
        if (m_Dead)
            return;

        // Checks if the player is dead, if true then set player state as dead
        if (m_Stats.IsDead() && !m_Dead)
        {
            SetState(State.dead);
            m_Dead = true;
            return;
        }

        // Makes sure animator knows that player has shield equipped
        m_Anim.SetBool("Shield", m_Shield);

        // If the lockon button is pressed, set lock_on operations
        if (input.LockOn() && m_Camera.GetComponent<ThirdPersonCamera>().LockOn() && !m_LockedOn)
        {
            m_LockedOn = true;
            m_Anim.SetBool("LockedOn", m_LockedOn);
        }
        // unlock player
        else if(input.LockOn() && m_LockedOn)
        {
            m_LockedOn = false;
            m_Anim.SetBool("LockedOn", m_LockedOn);
            m_Camera.GetComponent<ThirdPersonCamera>().LockOff();
        }
        
    }

    /**
     * What happens every fixed amount of frames
     * 
     * Executes the state that the player is in
     */
    private void FixedUpdate()
    {
        if(m_Paused)
        {
            return;
        }
        if (m_LockedOn)
        {
            // Sets the camera in its lockon position
            LockedOn();
        }
        ExecuteState();
    }

    /** 
     * Sets the player's state
     * 
     * t_State : the state that they player will be set as
     */
    private void SetState(State t_State)
    {
        m_PrevState = m_State;
        m_State = t_State;

        switch (m_State)
        {
            case State.idle:
                m_Run = false;
                m_Anim.SetFloat("MovementSpeed", 0);
                m_Anim.SetFloat("LockedOnHori", 0);
                m_Anim.SetFloat("LockedOnVert", 0);
                break;

            case State.walking:
                break;

            case State.dodge:
                Vector3 direction = new Vector3(input.Hori(), 0, input.Vert());
                if (direction == Vector3.zero)
                {
                    direction = Camera.main.transform.forward;
                }
                m_DodgeDirection = direction.normalized / 2;
                m_AnimFlags.DodgeStart();
                break;

            case State.dead:
                m_Anim.SetTrigger("Dead");
                break;

            case State.throwing:
                GameObject.Find("GUI").GetComponent<GUIManager>().AimOn();
                m_Anim.SetTrigger("StartAim");
                m_Anim.SetBool("Aim", true);
                break;

            case State.recall:
                m_Anim.SetTrigger("Recall");
                m_ShieldController.StartRecall();
                break;

            case State.combat:
                if(!m_LockedOn)
                    transform.eulerAngles = new Vector3(0, m_Camera.transform.eulerAngles.y, 0);
                m_Anim.SetTrigger("Attack");
                m_Anim.SetBool("Combat", true);
                m_TimeBetweenCombo = Time.time;
                m_CombatBuffered = false;
                m_AnimFlags.ResetCombat();
                m_AnimFlags.CombatStart();
                break;
                
            case State.block:
                m_Anim.SetBool("Block", true);
                break;

            case State.hit:
                m_Anim.SetTrigger("Hit");
                StartCoroutine(m_Camera.GetComponent<ThirdPersonCamera>().Shake(0.05f, 0.005f));
                break;

            default:
                break;
        }
    }

    /**
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

            case State.throwing:
                Throwing();
                break;

            case State.dead:
                break;

            case State.recall:
                Recall();
                break;

            case State.combat:
                Combat();
                break;

            case State.block:
                Block();
                break;

            case State.hit:
                Hit();
                break;

            default:
                break;
        }
    }

    /**
     * Player actions for when they are idle
     * 
     * states that can be transitioned to : Dodge, Combat, Block, Aim, Recall, Movement
     */
    private void Idle()
    {

        // Change to Dodge
        if (input.Dodge())
        {
            SetState(State.dodge);
        }
        // Change to Combat
        else if (input.Melee())
        {
            SetState(State.combat);
        }
        // Change to Block
        else if (m_Shield && input.Block())
        {
            SetState(State.block);
        }
        // Change to Aim
        else if (m_Shield && input.Aim())
        {
            SetState(State.throwing);
        }
        // Change to Recall
        else if (!m_Shield && input.Recall())
        {
            SetState(State.recall);
        }
        // Change to Movement
        else if (input.Vert() != 0 || input.Hori() != 0)
        {
            SetState(State.walking);
        }
    }

    /**
     * Player actions for when they are walking/running
     * 
     * states that can be transitioned to : Dodge, Combat, Block, Aim, Recall, Idle
     */
    private void Walking()
    {
        // Change to Dodge
        if (input.Dodge())
        {
            SetState(State.dodge);
            return;
        }
        // Change to Combat
        else if (input.Melee())
        {
            SetState(State.combat);
            return;
        }
        // Change to Block
        else if (m_Shield && input.Block())
        {
            SetState(State.block);
            return;
        }
        // Change to Aim
        else if (m_Shield && input.Aim())
        {
            SetState(State.throwing);
            return;
        }
        // Change to Recall
        else if (!m_Shield && input.Recall())
        {
            SetState(State.recall);
            return;
        }
        // Change to Idle
        else if (input.Vert() == 0 && input.Hori() == 0)
        {
            SetState(State.idle);
            return;
        }

        if (m_LockedOn)
        {
            //Forward and backward movement
            float fbspeed = LOCK_SPEED * input.Vert();
            transform.Translate(transform.forward * fbspeed * Time.deltaTime, Space.World);

            //Left and right movement
            float lrspeed = LOCK_SPEED * input.Hori();
            transform.Translate(transform.right * lrspeed * Time.deltaTime, Space.World);

            //Set Animations
            m_Anim.SetFloat("LockedOnHori", input.Hori());
            m_Anim.SetFloat("LockedOnVert", input.Vert());
        }
        else
        {
            //Get target direction
            Vector2 inputDir = new Vector2(input.Hori(), input.Vert());
            float targetRot = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + m_Camera.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref m_TurnVel, TURN_TIME);

            //If the analog stick is near the middle, turn off run
            m_Run = ((input.Hori() < 0.25f && input.Hori() > -0.25f) && (input.Vert() < 0.25f && input.Vert() > -0.25f)) ? false : m_Run;

            if (input.Run())
                m_Run = !m_Run;

            float speed = (m_Run ? RUN_SPEED : WALK_SPEED) * inputDir.magnitude;
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

            //Set Animation
            m_Anim.SetFloat("MovementSpeed", speed / 6);
        }


    }

    /**
     * What the player does when they are dodging
     * 
     * states that can be transitioned to : Combat, Aim, Block, Recall, Previous
     */
    private void Dodge()
    {
        // Buffer attack for after dodge attack
        if (input.Melee() && !m_CombatBuffered && Time.time - m_TimeBetweenCombo > DELAY_BETWEEN_COMBO)
        {
            m_CombatBuffered = true;
        }
        // Next states after roll ends
        if (m_AnimFlags.DodgeStatus())
        {
            m_Anim.SetBool("Dodge", false);

            // Change to Combat
            if (m_CombatBuffered)
            {
                SetState(State.combat);
            }
            // Change to Aim
            else if (m_Shield && input.Aim())
            {
                SetState(State.throwing);
            }
            // Change to Block
            else if (m_Shield && input.Block())
            {
                SetState(State.block);
            }
            // Change to Recall
            else if (!m_Shield && input.Recall())
            {
                SetState(State.recall);
            }
            // Change to Movement
            else if (input.Vert() != 0 || input.Hori() != 0)
            {
                SetState(State.walking);
            }
            // Change to Idle
            else
            {
                SetState(State.idle);
            }
            return;
        }

        if (m_LockedOn)
        {
            //Left and right movement
            float lrspeed = JUKE_SPEED * m_DodgeDirection.x;
            transform.Translate(transform.right * lrspeed * Time.deltaTime, Space.World);

            //Forward and backward movement
            float fbspeed = JUKE_SPEED * m_DodgeDirection.z;
            transform.Translate(transform.forward * fbspeed * Time.deltaTime, Space.World);

            m_Anim.SetBool("Dodge", true);
            m_Anim.SetFloat("LockedOnHori", m_DodgeDirection.x);
            m_Anim.SetFloat("LockedOnVert", m_DodgeDirection.z);
        }
        else
        {
            // Gets target direction from initial direction
            float targetRot = Mathf.Atan2(m_DodgeDirection.x, m_DodgeDirection.z) * Mathf.Rad2Deg;
            // If a direction is inputted, move relative to camera
            if(m_DodgeDirection.x != 0 || m_DodgeDirection.z != 0)
            {
                targetRot += m_Camera.transform.eulerAngles.y;
            }
            // Rotates towards dirction
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref m_TurnVel, TURN_TIME);
            float speed = ROLL_SPEED * m_DodgeDirection.magnitude;
            if(m_Run)
            {
                speed *= 1.5f;
            }
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            m_Anim.SetBool("Dodge", true);
        }
    }

    /**
     * What the player does when they are Aim/Throwing
     * 
     * states that can be transitioned to : Dodge, Block, Previous
     */
    private void Throwing()
    {
        //cancel aiming
        if (!input.Aim() || input.AimCancel())
        {
            GameObject.Find("GUI").GetComponent<GUIManager>().AimOff();
            m_Anim.SetBool("Aim", false);
            m_Camera.GetComponent<ThirdPersonCamera>().AimOff();

            if (input.Dodge())
            {
                SetState(State.dodge);
            }
            else if (input.Block())
            {
                SetState(State.block);
            }
            else if (input.Vert() == 0 && input.Hori() == 0)
            {
                SetState(State.idle);
            }
            else
            {
                SetState(State.walking);
            }
            return;
        }


        //throw
        if (input.Melee())
        {
            GameObject.Find("GUI").GetComponent<GUIManager>().AimOff();
            m_Anim.SetBool("Aim", false);
            m_Anim.SetTrigger("Throw");
            m_Shield = false;
            //m_Camera.GetComponent<ThirdPersonCamera>().AimOff();
            if (input.Dodge())
                SetState(State.dodge);
            else if (input.Block())
                SetState(State.block);
            else
                SetState(m_PrevState);
            return;
        }
        //Rotate and move around when aiming
        else
        {
            m_Camera.GetComponent<ThirdPersonCamera>().AimOn();
            // aim stuff
            float targetRot = m_Camera.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref m_TurnVel, TURN_TIME);

            float fbspeed = LOCK_SPEED * input.Vert();
            transform.Translate(transform.forward * fbspeed * Time.deltaTime, Space.World);

            //Left and right movement
            float lrspeed = LOCK_SPEED * input.Hori();
            transform.Translate(transform.right * lrspeed * Time.deltaTime, Space.World);

            m_Anim.SetFloat("LockedOnHori", input.Hori());
            m_Anim.SetFloat("LockedOnVert", input.Vert());
        }

    }

    /**
     * What the player does when they are recalling the shield
     * 
     * states that can be transitioned to : Dodge, Block, Combat, Previous
     */
    private void Recall()
    {
        // If the shield has been caught
        if(m_ShieldController.InHand())
        {
            m_Anim.SetTrigger("Caught");
            m_Shield = true;

            if (input.Dodge())
                SetState(State.dodge);
            else if (input.Block())
                SetState(State.block);
            else if (input.Melee())
                SetState(State.combat);
            else if (input.Vert() == 0 && input.Hori() == 0)
                SetState(State.idle);
            else
                SetState(State.walking);
        }
    }

    /**
     * What the player does when they are in combat
     * 
     * states that can be transitioned to : Dodge, Block, Combat, Previous
     */
    private void Combat()
    {
        //slide on attack frames to make attacks feel natural
        if (m_AnimFlags.CombatMove())
        {
            float speed = 1.5f;
            if(m_Run)
            {
                speed = 2f;
            }
            else if(m_PrevState == State.idle)
            {
                speed = 1f;
            }
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }

        // if hitbox is active, check combat
        if(m_AnimFlags.CombatHitboxActive())
        {
            // Detect enemies in range
            List<Collider> hitEnemies = new List<Collider>();
            if(m_Shield)
            {
                hitEnemies.AddRange(Physics.OverlapSphere(m_ShieldPoint.position, SHIELD_RANGE, m_EnemyLayer));
            }
            else
            {
                hitEnemies.AddRange(Physics.OverlapSphere(m_LeftFist.position, ATTACK_RANGE, m_EnemyLayer));
                hitEnemies.AddRange(Physics.OverlapSphere(m_RightFist.position, ATTACK_RANGE, m_EnemyLayer));
                hitEnemies.AddRange(Physics.OverlapSphere(m_Foot.position, ATTACK_RANGE, m_EnemyLayer));
            }

            // List of all enemies hit and are distinct
            List<Collider> distinct = hitEnemies.Distinct().ToList();

            //Damage/effects enemies
            foreach (Collider enemy in distinct)
            {
                
                if (m_Shield)
                {
                    enemy.GetComponent<Rigidbody>().AddForce(transform.forward * SHIELD_KNOCKBACK, ForceMode.Impulse);
                }
                else
                {
                    enemy.GetComponent<Rigidbody>().AddForce(transform.forward * FIST_KNOCKBACK, ForceMode.Impulse);
                }

                // skip if enemy has already been damaged
                if (m_AlreadyHit.ContainsKey(enemy.gameObject.name))
                {
                    continue;
                }

                if(m_Shield)
                {
                    enemy.GetComponent<EnemyController>().GotHit(SHIELD_DAMAGE, true);
                    StartCoroutine(m_Camera.GetComponent<ThirdPersonCamera>().Shake(0.05f, 0.01f));
                }
                else
                {
                    enemy.GetComponent<EnemyController>().GotHit(FIST_DAMAGE, false);
                    StartCoroutine(m_Camera.GetComponent<ThirdPersonCamera>().Shake(0.05f, 0.005f));
                }
                m_AlreadyHit.Add(enemy.gameObject.name, enemy.gameObject);
                
            }
        }        

        //Buffers next hit
        if (input.Melee() && !m_CombatBuffered && Time.time - m_TimeBetweenCombo > DELAY_BETWEEN_COMBO )
        {
            m_CombatBuffered = true;
        }

        //Animation Cancels
        if (input.Dodge())
        {
            m_Anim.SetBool("Combat", false);
            m_Anim.SetBool("CombatSpecial", false);
            m_AnimFlags.ResetFlags();
            SetState(State.dodge);
            m_AlreadyHit.Clear();
            return;
        }

        // Sets up next state after attack is over
        if (!m_AnimFlags.CombatStatus())
        {
            m_Anim.SetBool("Combat", false);
            if (m_CombatBuffered)
            {
                SetState(State.combat);
            }
            else if (input.Vert() == 0 && input.Hori() == 0)
            {
                SetState(State.idle);
            }
            else
            {
                SetState(State.walking);
            }
            m_AlreadyHit.Clear();
        }
    }

    /**
     * What the player does when they are blocking
     * 
     * states that can be transitioned to : Dodge, Combat, Previous
     */
    private void Block()
    {
        if(!input.Block() || input.BlockCancel())
        {
            m_Anim.SetBool("Block", false);
            if (input.Dodge())
                SetState(State.dodge);
            else if (input.Melee())
                SetState(State.combat);
            else if (input.Vert() == 0 && input.Hori() == 0)
            {
                SetState(State.idle);
            }
            else
            {
                SetState(State.walking);
            }
        }
    }

    /**
     * What the player does when they are hit
     * 
     * states that can be transitioned to : Idle
     */
    private void Hit()
    {
        if(!m_AnimFlags.HitStatus())
        {
            SetState(State.idle);
        }
    }

    /**
     * When the lockon button is pressed, how it affects the player
     * 
     * Will turn the player towards the locked on target
     */
    private void LockedOn()
    {
        // get focus that should be faced towards
        Transform thing = m_Camera.GetComponent<ThirdPersonCamera>().GetLockedOnTarget();
        // Unlock if target is dead
        if (thing == null || thing.GetComponent<Stats>().IsDead())
        {
            m_LockedOn = false;
            m_Anim.SetBool("LockedOn", m_LockedOn);
            m_Camera.GetComponent<ThirdPersonCamera>().LockOff();
            return;
        }
        //Rotates towards target
        Vector3 lookdir = thing.position - transform.position;
        lookdir.Normalize();
        lookdir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookdir);
        m_Anim.SetBool("LockedOn", m_LockedOn);
    }

    /**
     * Draws Gizmos when object is selected 
     * 
     * Draw the hit boxes
     */
    private void OnDrawGizmos()
    {
        if(m_LeftFist == null || m_RightFist == null || m_Foot == null)
        {
            return;
        }
        if(m_Shield)
        {
            Gizmos.DrawWireSphere(m_ShieldPoint.position, SHIELD_RANGE);
        }
        else
        {
            Gizmos.DrawWireSphere(m_LeftFist.position, ATTACK_RANGE);
            Gizmos.DrawWireSphere(m_RightFist.position, ATTACK_RANGE);
            Gizmos.DrawWireSphere(m_Foot.position, ATTACK_RANGE);
        }
    }

    /**
     * Status if player is aiming
     * 
     * return : if player is aiming or not
     */
    public bool isAiming()
    {
        if(m_State == State.throwing)
        {
            return true;
        }
        return false;
    }

    /**
     * Status if player has gotten hit
     * 
     * t_Damage : damage that will be dealt
     * t_Stagger : If player will be staggered
     */
    public void GotHit(int t_Damage, bool t_Stagger = false)
    {
        // If in dodge, nothing to worry about
        if(m_State == State.dodge && m_AnimFlags.DodgeStatus())
        {
            return;
        }

        // If attack is not staggerable and blocking
        else if (m_State == State.block && !t_Stagger)
        {
            m_Stats.Damage(t_Damage/4);
            return;
        }
        m_Stats.Damage(t_Damage);
        // If dead after damage don't do anything
        if (m_Stats.IsDead())
        {
            return;
        }
        // Sets stagger
        m_AnimFlags.HitStart();
        SetState(State.hit);
    }

    /**
     * Set player to pause
     */
    public void Pause()
    {
        m_Paused = true;
    }

    /**
     * Set player to unpause
     */
    public void Resume()
    {
        m_Paused = false;
    }

}
