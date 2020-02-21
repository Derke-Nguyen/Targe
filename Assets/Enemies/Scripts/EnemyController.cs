/**
 * File: EnemyController.cs 
 * Author: Derek Nguyen
 * 
 * Base Class for EnemyController
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //States that the enemy can be in
    public enum State { idle, walking, hit, combat, dead, frozen };

    //The enemy's current state as well as the state that it was in
    private State m_State = State.idle;
    private State m_PrevState = State.idle;

    // Enemy's animator
    protected Animator m_Anim;

    //If the enemy is dead or not
    private bool m_Dead;

    //Used for when frozen by shield
    protected Rigidbody m_RigidBody;

    //The enemy's stats
    protected Stats m_Stats;

    //Used for when enemy dies, remove collider
    protected CapsuleCollider m_CapsuleCollider;

    //Animation flags for determining end of animations
    protected EnemyAnimationFlags m_AnimFlags;

    //Keep a reference to player position for AI detection
    protected Transform m_PlayerLocation;

    //Reference to the player layer so only attack things in this layer
    public LayerMask m_PlayerLayer;

    //Enemy's Attack Damage
    protected int m_AttackDamage = 10;
    //Enemy's Attack Knockback
    protected float m_Knockback = 1.5f;
    //Enemy's Attack Range
    protected float m_HitSphereRange = 0.2f;
    //Make sure that only hit player once
    private Dictionary<string, GameObject> m_AlreadyHit = new Dictionary<string, GameObject>();

    //Reference to the hitpoint that checks for player range
    [SerializeField]
    protected Transform m_Attackpoint;

    //Determines if player is within follow range
    protected float m_DetectRange = 10f;
    //Determines if player is within attack range
    protected float m_AttackRange = 1.2f;
    //How fast enemy moves
    protected float m_MoveSpeed = 3f;
    //How fast enemy can turn/rotate
    protected float m_TurnTime = 0.2f;
    //The speed which the enemy rotates
    protected float m_TurnVel;

    //Rotation that players will rotate towards before attaacking
    protected float m_ViewRange = 0.025f;

    //If the enemy is paused
    protected bool m_Pause = false;

    //If can be blocked
    protected bool m_Unblockable = false;

    /**
     * What happens on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    public virtual void Start()
    {
        m_Anim = gameObject.GetComponentInChildren<Animator>();
        m_AnimFlags = gameObject.GetComponentInChildren<EnemyAnimationFlags>();
        m_RigidBody = gameObject.GetComponent<Rigidbody>();
        m_Stats = GetComponent<Stats>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_PlayerLocation = GameObject.Find("player").GetComponent<Transform>();
    }

    /**
     * What happens every frame
     * 
     * Do nothing if game is paused
     * Do nothing if enemy is dead
     * If recently dead, play death animation
     */
    void Update()
    {
        if (m_Pause)
        {
            return;
        }
        //No character control if player is already dead
        if (m_Dead)
            return;

        // Checks if the enemy is dead, if true then set enemy state as dead
        if (m_Stats.IsDead() && !m_Dead)
        {
            SetState(State.dead);
            m_Dead = true;
            return;
        }
    }

    /**
     * What happens every fixed amount of frames
     * 
     * If game is not paused, run state
     * else, don't do anything
     */
    private void FixedUpdate()
    {
        if(!m_Pause)
        {
            ExecuteState();
        }
    }

    /**
     * Sets the enemy's state
     * 
     * t_state : the state that they enemy will be set as
     */
    protected void SetState(State t_State)
    {
        m_PrevState = m_State;
        m_State = t_State;

        switch (m_State)
        {
            //Set walking animation
            case State.walking:
                m_Anim.SetBool("move", true);
                break;

            //Set death animation and turn off colliders
            case State.dead:
                m_Anim.SetTrigger("death");
                m_RigidBody.isKinematic = true;
                m_CapsuleCollider.enabled = false;
                break;
            
            //Set attack animation
            case State.combat:
                m_Anim.SetTrigger("attack");
                m_AnimFlags.CombatStart();
                break;
            
            //Set hit animation
            case State.hit:
                m_Anim.SetTrigger("hit");
                m_AnimFlags.HitStart();
                break;
            
            //Do nothing for all other states
            default:
                break;
        }
    }

    /**
     * Executes enemy instructions based on what state the enemy is in 
    */
    protected virtual void ExecuteState()
    {
        switch (m_State)
        {
            case State.idle:
                Idle();
                break;

            case State.walking:
                Walking();
                break;

            case State.hit:
                Hit();
                break;

            case State.dead:
                break;

            case State.combat:
                Combat();
                break;

            default:
                break;
        }
    }

    /**
     * Enemy actions for when they are idle
     * 
     * states that can be transitioned to : Walking, Combat
     */
    protected virtual void Idle()
    {
        //Determine if enemy is facing the player
        Vector3 target = m_PlayerLocation.position - transform.position;
        //determine distance between player and enemy
        float distance = Mathf.Abs(target.magnitude);
        target.Normalize();
        float targetRot = Mathf.Acos(Vector3.Dot(target, transform.forward));
        bool facing = (targetRot < m_ViewRange) ? true : false;
        //If not facing, rotate enemy towards player
        if(!facing)
        {
            Vector2 targetLocation = new Vector2(target.x, target.z);
            float targetLook = Mathf.Atan2(targetLocation.x, targetLocation.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetLook, ref m_TurnVel, m_TurnTime);
        }
        //If player is in detect range and not in attack range, move towards player
        if (distance <= m_DetectRange && distance > m_AttackRange)
        {
            SetState(State.walking);
        }
        //If player is in attack range and enemy is facing player, attack
        else if(distance <= m_AttackRange && facing)
        {
            SetState(State.combat);
        }
        
    }

    /**
     * Enemy actions for when they are walking
     * 
     * states that can be transitioned to : Idle
     */
    protected virtual void Walking()
    {
        //Get distance towards player
        Vector3 distance = m_PlayerLocation.position - transform.position;

        //Rotate
        Vector2 targetLocation = new Vector2(distance.x, distance.z);
        float targetRot = Mathf.Atan2(targetLocation.x, targetLocation.y) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref m_TurnVel, m_TurnTime);

        //Move
        transform.Translate(transform.forward * m_MoveSpeed * Time.deltaTime, Space.World);

        //If player is too far away or if in attack range, move back to idle
        if (Mathf.Abs(distance.magnitude) > m_DetectRange || Mathf.Abs(distance.magnitude) <= m_AttackRange)
        {
            m_Anim.SetBool("move", false);
            SetState(State.idle);
        }
    }

    /**
     * Enemy actions for when they are hit
     * 
     * states that can be transitioned to : Idle
     */
    protected virtual void Hit()
    {
        //When hit animation ends, then move back to idle animation, otherwise don't do anything
        if(m_AnimFlags.HitStatus())
        {
            SetState(State.idle);
        }
    }

    /**
     * Enemy actions for when they are in combat phase
     * 
     * states that can be transitioned to : Idle
     */
    protected virtual void Combat()
    {
        //If during frames when damage can be done
        if (m_AnimFlags.CombatHitBox())
        {
            // Detect enemies in range
            List<Collider> hitPlayer = new List<Collider>();
            hitPlayer.AddRange(Physics.OverlapSphere(m_Attackpoint.position, m_HitSphereRange, m_PlayerLayer));



            //Damage/effects player
            foreach (Collider player in hitPlayer)
            {
                if (m_AlreadyHit.ContainsKey(player.gameObject.name))
                {
                    continue;
                }
                //Knockback
                player.GetComponent<Rigidbody>().AddForce(transform.forward * m_Knockback, ForceMode.Impulse);
                //Deal Damage
                player.GetComponent<PlayerController>().GotHit(m_AttackDamage, m_Unblockable);

                //Add to dictionary so player won't be hit multiple times
                m_AlreadyHit.Add(player.gameObject.name, player.gameObject);
            }
        }
        //end of combat animations
        else if (!m_AnimFlags.CombatStatus())
        {
            //clear dictionary so player won't be hit again
            m_AlreadyHit.Clear();

            //Check distance of player
            float distance = Mathf.Abs(Vector3.Distance(m_PlayerLocation.position, transform.position));
            Vector3 target = m_PlayerLocation.position - transform.position;
            target.Normalize();
            float targetRot = Mathf.Acos(Vector3.Dot(target,transform.forward)/(distance));
            //If still in range, attack again, else just go back to idle
            if (distance <= m_AttackRange && targetRot == 0)
            {
                SetState(State.combat);
            }
            else
            {
                SetState(State.idle);
            }
        }
    }

    /**
     * When enemy is hit, do these operations
     * 
     * t_Damage : The damage that will be dealt
     * t_Stagger : If move will stagger or not
     */ 
    public virtual void GotHit(int t_Damage, bool t_Stagger = false)
    {
        m_Stats.Damage(t_Damage);
        if(m_State == State.frozen)
        {
            return;
        }
        SetState(State.hit);
    }

    /**
     * When enemy is frozen, set to frozen state and make enemy not able to move
     */
    public void Frozen()
    {
        SetState(State.frozen);
        m_RigidBody.isKinematic = true;
    }

    /**
     * When enemy is unfrozen, set to previous state and make enemy be able to move again
     */
    public void UnFrozen()
    {
        if(m_State == State.frozen)
        {
            SetState(m_PrevState);
        }
        m_RigidBody.isKinematic = false;
    }

    /**
     * When game is paused, let enemy pause as well
     */
    public void Pause()
    {
        m_Pause = true;
    }

    /**
     * When game is unpaused, let enemy unpause as well
     */
    public void Resume()
    {
        m_Pause = false;
    }

    private void OnDrawGizmos()
    {
        if (m_Attackpoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(transform.position, m_AttackRange);
        Gizmos.DrawWireSphere(m_Attackpoint.position, m_HitSphereRange);
    }
}
