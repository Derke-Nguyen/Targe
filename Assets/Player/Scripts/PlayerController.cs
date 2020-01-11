using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Possible States the player can be in
    public enum State { idle, walking, dodge, hit, dead, throwing, recall , combat, block};

    // Player's current state
    private State m_State = State.idle;
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
    private const float LOCK_SPEED = 1f;
    private const float TURN_TIME = 0.2f;
    private bool m_Run = false;
    private float m_TurnVel;
    private bool m_LockedOn;
    #endregion

    #region Dodge
    private Vector3 m_DodgeDirection = Vector3.zero;
    private const float ROLL_SPEED = 8f;
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
    private const float DELAY_BETWEEN_COMBO = 0.2f;
    private float SPECIAL_COMBO_DELAY = 0.5f;
    private bool m_CombatBuffered = false;
    [SerializeField]
    private bool m_CombatSpecial = false;
    private Transform m_AttackPoint;
    private float ATTACK_RANGE = 0.4f;
    public LayerMask m_EnemyLayer;
    private int m_AttackDamage = 5;
    #endregion

    /* What happesn on start frame
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
        m_AttackPoint = GameObject.Find("AttackPoint").transform;
    }

    /* What happens every frame
     * 
     * Checks for certain inputs that will override all states
     * Also tells animator if there is a shield equipped
     * Also checks if player is dead
     */
    private void Update()
    {
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

        m_Anim.SetBool("Shield", m_Shield);

        // If the lockon button is pressed, set lock_on operations
        if (input.LockOn() && m_Camera.GetComponent<ThirdPersonCamera>().LockOn() && !m_LockedOn)
        {
            m_LockedOn = true;
            m_Anim.SetBool("LockedOn", m_LockedOn);
        }
        else if(input.LockOn() && m_LockedOn)
        {
            m_LockedOn = false;
            m_Anim.SetBool("LockedOn", m_LockedOn);
            m_Camera.GetComponent<ThirdPersonCamera>().LockOff();
        }
        
    }

    /* What happens every fixed amount of frames
     * 
     * Executes the state that the player is in
     */
    private void FixedUpdate()
    {
        ExecuteState();
    }

    /* Sets the player's state
     * 
     * t_state : the state that they player will be set as
     */
    private void SetState(State t_state)
    {
        if(m_State == State.idle || m_State == State.walking)
            m_PrevState = m_State;
        m_State = t_state;

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
                    direction = gameObject.transform.forward;
                }
                m_DodgeDirection = direction.normalized / 2;
                m_AnimFlags.DodgeStart();
                break;

            case State.dead:
                m_Anim.SetTrigger("Dead");
                break;

            case State.throwing:
                m_Anim.SetTrigger("StartAim");
                m_Anim.SetBool("Aim", true);
                break;

            case State.recall:
                m_Anim.SetTrigger("Recall");
                m_ShieldController.StartRecall();
                break;

            case State.combat:
                m_Anim.SetBool("CombatSpecial", m_CombatSpecial);
                m_Anim.SetBool("Combat", true);
                m_AnimFlags.CombatStart();
                m_AnimFlags.CombatWindUpStart();
                m_TimeBetweenCombo = Time.time;
                m_CombatBuffered = false;
                break;
                
            case State.block:
                m_Anim.SetBool("Block", true);
                break;

            default:
                break;
        }
    }

    /* Executes player instructions based on what state the player is in
     * 
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

            default:
                break;
        }
    }

    /* Player actions for when they are idle
     * 
     * states that can be transitioned to : Dodge, Combat, Block, Aim, Recall, Movement
     */
    private void Idle()
    {
        // Sets the camera in its lockon position
        if (m_LockedOn)
        {
            LockedOn();
        }

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

    /* Player actions for when they are walking/running
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
            LockedOn();

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
            Vector2 inputDir = new Vector2(input.Hori(), input.Vert());
            float targetRot = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + m_Camera.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref m_TurnVel, TURN_TIME);

            if (input.Run())
                m_Run = !m_Run;

            float speed = (m_Run ? RUN_SPEED : WALK_SPEED) * inputDir.magnitude;
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

            //Set Animation
            m_Anim.SetFloat("MovementSpeed", speed / 6);
        }


    }

    /* What the player does when they are dodging
     * 
     * states that can be transitioned to : Combat, Aim, Block, Recall, Previous
     */
    private void Dodge()
    {
        if (m_AnimFlags.DodgeStatus())
        {
            m_Anim.SetBool("Dodge", false);

            // Change to Combat
            if (input.Melee())
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
            // Change to Idle/Movement
            else
            {
                SetState(m_PrevState);
            }
            return;
        }

        if (m_LockedOn)
        {
            LockedOn();

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
            float targetRot = Mathf.Atan2(m_DodgeDirection.x, m_DodgeDirection.z) * Mathf.Rad2Deg + m_Camera.transform.eulerAngles.y;
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

    /* What the player does when they are Aim/Throwing
    * 
    * states that can be transitioned to : Dodge, Block, Previous
    */
    private void Throwing()
    {
        //cancel
        if (!input.Aim() || input.AimCancel())
        {
            m_Anim.SetBool("Aim", false);
            m_Camera.GetComponent<ThirdPersonCamera>().AimOff();

            if (input.Dodge())
                SetState(State.dodge);
            else if (input.Block())
                SetState(State.block);
            else
                SetState(m_PrevState);
            return;
        }


        //throw
        if (input.Melee())
        {
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

    /* What the player does when they are recalling the shield
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
            else
                SetState(m_PrevState);
        }
    }

    /* What the player does when they are in combat
    * 
    * states that can be transitioned to : Dodge, Block, Combat, Previous
    */
    private void Combat()
    {
        if (m_AnimFlags.CombatWindUp())
        {
            float speed = 1f;
            if(m_Run)
            {
                speed = 4f;
            }
            else if(m_PrevState == State.idle)
            {
                speed = 0.1f;
            }
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }

        if(m_AnimFlags.CombatHit())
        {
            // Detect enemies in range
            Collider[] hitEnemies = Physics.OverlapSphere(m_AttackPoint.position, ATTACK_RANGE, m_EnemyLayer);

            //Damage/effects enemies
            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<Stats>().Damage(m_AttackDamage);
                Debug.Log("Hit " + enemy.name);
            }
        }
        

        //Next states
        if (input.Melee() && !m_CombatBuffered && Time.time - m_TimeBetweenCombo > DELAY_BETWEEN_COMBO )
        {
            m_CombatBuffered = true;
            m_CombatSpecial = false;
            if (Time.time - m_TimeBetweenCombo >= SPECIAL_COMBO_DELAY && !m_Shield)
            {
                m_CombatSpecial = true;
            }
            return;
        }

        if (m_AnimFlags.CombatStatus())
        {
            m_Anim.SetBool("Combat", false);
            m_Anim.SetBool("CombatSpecial", false);

            if (input.Dodge())
            {
                SetState(State.dodge);
            }
            else if(input.Block())
            {
                SetState(State.block);
            }
            else if (m_CombatBuffered)
            {
                SetState(State.combat);
            }
            else
            {
                SetState(m_PrevState);
            }
        }
    }

    /* What the player does when they are blocking
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
            else
                SetState(m_PrevState);
        }
    }

    /* When the lockon button is pressed, how it affects the player
     * 
     * Will turn the player towards the locked on target
     */
    private void LockedOn()
    {
        Transform thing = m_Camera.GetComponent<ThirdPersonCamera>().GetLockedOnTarget();
        if (thing == null || thing.gameObject.GetComponent<Stats>().IsDead())
        {
            m_LockedOn = false;
            m_Anim.SetBool("LockedOn", m_LockedOn);
            m_Camera.GetComponent<ThirdPersonCamera>().LockOff();
            return;
        }
        //Vector3 thingDir = new Vector3(thing.position.x, transform.position.y, thing.position.z);
        transform.LookAt(thing.position);
        m_Anim.SetBool("LockedOn", m_LockedOn);
    }

    private void OnDrawGizmosSelected()
    {
        if(m_AttackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(m_AttackPoint.position, ATTACK_RANGE);
    }
}
