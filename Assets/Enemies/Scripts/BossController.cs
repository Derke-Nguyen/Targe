/**
 * File: BossController.cs 
 * Author: Derek Nguyen
 * 
 * Controller for the Boss
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    // States the boss can be in
    public enum BossState { idle, walking, hit, combat, special, dead };

    // The boss's current state
    [SerializeField]private BossState m_BossState = BossState.idle;

    // The hitbox radius of the boss's kick
    private float m_FootRange = 0.2f;
    
    // The transform of the foot for kick
    [SerializeField] private Transform m_Footpoint;
    // The transform of the tip of the sword
    [SerializeField] private Transform m_Tip;

    // Heavy Damage
    private int m_HardDamage = 25;
    // Kick Damage
    private int m_FootDamage = 5;

    // Heavy Knockback
    private float m_HeavyKnockback = 3f;

    // Regen of the boss during special
    private int m_Regen = 1;
    
    // If boss has been defeated
    private bool m_Defeated;

    // Dictionary to keep track if player has already been hit
    private Dictionary<string, GameObject> m_AlreadyHit = new Dictionary<string, GameObject>();

    // Bool to keep track if heavy attack or normal
    private bool m_Heavy = false;

    /**
     * What happesn on start frame
     * 
     * Gathers all components that are needed and initializes the object
     * Sets stats specific for the boss
     */
    public override void Start()
    {
        base.Start();
        m_AttackDamage = 15;
        m_Knockback = 2;
        m_HitSphereRange = 0.35f;
        m_DetectRange = 30f;
        m_AttackRange = 2f;
        m_MoveSpeed = 2f;
        m_TurnTime = 0.2f;
        m_ViewRange = 0.8f;
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
        if(m_Pause)
        {
            return;
        }

        //No character control if boss is already dead
        if (m_Defeated)
            return;

        // Checks if the enemy is dead, if true then set enemy state as dead
        if (m_Stats.IsDead() && !m_Defeated)
        {
            SetState(BossState.dead);
            m_Defeated = true;
            return;
        }
    }

    /**
     * Sets the enemy's state
     * 
     * t_state : the state that they enemy will be set as
     */
    private void SetState(BossState t_State)
    {
        m_BossState = t_State;

        switch (m_BossState)
        {
            case BossState.idle:
                break;

            case BossState.walking:
                m_Anim.SetBool("move", true);
                break;

            case BossState.dead:
                m_Anim.SetTrigger("death");
                m_RigidBody.isKinematic = true;
                m_CapsuleCollider.enabled = false;
                break;

            case BossState.combat:
                float temp = Random.Range(0, 100);
                // 64% chance that it will be a weak attack
                m_Heavy = (temp >= 65f) ? true : false;
                if (m_Heavy)
                    m_Anim.SetTrigger("hattack");
                else
                    m_Anim.SetTrigger("lattack");
                m_AnimFlags.CombatStart();
                break;

            case BossState.hit:
                m_Anim.SetTrigger("hit");
                m_AnimFlags.HitStart();
                break;

            case BossState.special:
                m_Anim.SetTrigger("sattack");
                m_AnimFlags.CombatStart();
                break;

            default:
                break;
        }
    }

    /**
     * Executes enemy instructions based on what state the enemy is in 
     */
    protected override void ExecuteState()
    {
        switch (m_BossState)
        {
            case BossState.idle:
                Idle();
                break;

            case BossState.walking:
                Walking();
                break;

            case BossState.hit:
                Hit();
                break;

            case BossState.dead:
                break;

            case BossState.combat:
                Combat();
                break;

            case BossState.special:
                Special();
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
    protected override void Idle()
    {
        // Get values of player's position
        Vector3 target = m_PlayerLocation.position - transform.position;
        float distance = Mathf.Abs(target.magnitude);
        target.Normalize();

        //Checks if facing player
        float targetRot = Mathf.Acos(Vector3.Dot(target, transform.forward));
        bool facing = (targetRot < m_ViewRange) ? true : false;
        if (!facing)
        {
            Vector2 targetLocation = new Vector2(target.x, target.z);
            float targetLook = Mathf.Atan2(targetLocation.x, targetLocation.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetLook, ref m_TurnVel, m_TurnTime);
        }
        
        //If within distance, then move towards the player
        if (distance <= m_DetectRange && distance > m_AttackRange)
        {
            SetState(BossState.walking);
        }
        //If facing, then attack
        else if (distance <= m_AttackRange && facing)
        {
            //30% chance of doing special
            if(m_Stats.GetPercentHealth() <= 0.3)
            {
                float temp = Random.Range(0, 100);
                if (temp > 70)
                    SetState(BossState.special);
                else
                    SetState(BossState.combat);
            }
            else
            {
                SetState(BossState.combat);
            }
        }

    }

    /**
     * Enemy actions for when they are walking
     * 
     * states that can be transitioned to : Idle
     */
    protected override void Walking()
    {
        Vector3 distance = m_PlayerLocation.position - transform.position;
        //Rotate
        Vector2 targetLocation = new Vector2(distance.x, distance.z);
        float targetRot = Mathf.Atan2(targetLocation.x, targetLocation.y) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref m_TurnVel, m_TurnTime);
        //Move
        transform.Translate(transform.forward * m_MoveSpeed * Time.deltaTime, Space.World);

        if (Mathf.Abs(distance.magnitude) > m_DetectRange || Mathf.Abs(distance.magnitude) <= m_AttackRange)
        {
            m_Anim.SetBool("move", false);
            SetState(BossState.idle);
        }
    }

    /**
     * Enemy actions for when they are hit
     * 
     * states that can be transitioned to : Idle
     */
    protected override void Hit()
    {
        Debug.Log(m_AnimFlags.HitStatus());
        if (m_AnimFlags.HitStatus())
        {
            SetState(BossState.idle);
        }
    }

    /**
     * Enemy actions for when they are in combat phase
     * 
     * states that can be transitioned to : Idle
     */
    protected override void Combat()
    {

        if (m_AnimFlags.CombatHitBox())
        {
            // Detect if player is in range
            List<Collider> hitPlayer = new List<Collider>();
            hitPlayer.AddRange(Physics.OverlapCapsule(m_Attackpoint.position, m_Tip.position, m_HitSphereRange, m_PlayerLayer));

            //Damage/effects player
            foreach (Collider player in hitPlayer)
            {
                if (m_AlreadyHit.ContainsKey(player.gameObject.name))
                {
                    continue;
                }
                //If heavy attack, then deal that damage and do knockback
                if (m_Heavy)
                {
                    player.GetComponent<PlayerController>().GotHit(m_HardDamage, true);
                    player.GetComponent<Rigidbody>().AddForce(transform.forward * m_HeavyKnockback, ForceMode.Impulse);
                }
                //Else just do normal amounts of damage
                else
                {
                    player.GetComponent<PlayerController>().GotHit(m_AttackDamage);
                    player.GetComponent<Rigidbody>().AddForce(transform.forward * m_Knockback, ForceMode.Impulse);
                }
                m_AlreadyHit.Add(player.gameObject.name, player.gameObject);
            }
        }

        //If the combat animation is over
        if (!m_AnimFlags.CombatStatus())
        {
            //Clear what's already been hit
            m_AlreadyHit.Clear();
            float distance = Mathf.Abs(Vector3.Distance(m_PlayerLocation.position, transform.position));
            Vector3 target = m_PlayerLocation.position - transform.position;
            target.Normalize();
            float targetRot = Mathf.Acos(Vector3.Dot(target, transform.forward) / (distance));
            if (distance <= m_AttackRange && targetRot == 0)
            {
                //Check weights and probability of doing the special
                if (m_Stats.GetPercentHealth() < 0.3f)
                {
                    float temp = Random.Range(0, 100);
                    if (temp > 85)
                        SetState(BossState.special);
                    else
                        SetState(BossState.combat);
                }
                else
                {
                    SetState(BossState.combat);
                }
            }
            else
            {
                SetState(BossState.idle);
            }
        }
    }

    /**
     * Enemy actions for when they are in combat phase
     * 
     * states that can be transitioned to : Idle
     */
    private void Special()
    {
        m_Anim.SetBool("block", true);
        if (m_AnimFlags.CombatHitBox())
        {
            // Detect enemies in range
            List<Collider> hitPlayer = new List<Collider>();
            hitPlayer.AddRange(Physics.OverlapSphere(m_Attackpoint.position, m_HitSphereRange, m_PlayerLayer));



            //Damage/effects enemies
            foreach (Collider player in hitPlayer)
            {
                player.GetComponent<Rigidbody>().AddForce(transform.forward * m_Knockback, ForceMode.Impulse);

                if (m_AlreadyHit.ContainsKey(player.gameObject.name))
                {
                    continue;
                }
                player.GetComponent<PlayerController>().GotHit(m_FootDamage, true);

                m_AlreadyHit.Add(player.gameObject.name, player.gameObject);
            }
        }
        //Regen after the attack animation
        if (!m_AnimFlags.CombatStatus())
        {
            m_Stats.Heal(m_Regen);
        }
        //Switch back to idle once healed to 80% health
        if(m_Stats.GetPercentHealth() >= 0.8)
        {
            SetState(BossState.idle);
            m_Anim.SetBool("block", false);
        }
    }

    /**
     * When enemy is hit, do these operations
     * 
     * t_Damage : The damage that will be dealt
     * t_Stagger : If move will stagger or not
     */
    public override void GotHit(int t_damage, bool m_unblockable = false)
    {
        m_Stats.Damage(t_damage);
        if(m_BossState == BossState.special && m_unblockable)
        {
            SetState(BossState.hit);
        }
    }

    /* Draws Gizmos when object is selected 
     * 
     * Draw the hit boxes
     */
    private void OnDrawGizmos()
    {
        if (m_Attackpoint == null || m_Footpoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(transform.position, m_AttackRange);
        //Gizmos.DrawWireSphere(m_Attackpoint.position, m_HitSphereRange);
        Gizmos.DrawWireSphere(m_Footpoint.position, m_FootRange);
    }
}
