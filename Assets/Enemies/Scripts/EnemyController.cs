﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum State { idle, walking, hit, combat, dead };

    private State m_State = State.idle;

    // Enemy's animator
    private Animator m_Anim;

    private bool m_Dead;

    private Rigidbody m_RigidBody;

    private Stats m_Stats;

    private BoxCollider m_BoxCollider;

    private EnemyAnimationFlags m_AnimFlags;

    private Transform m_PlayerLocation;

    public LayerMask m_PlayerLayer;

    private int m_AttackDamage = 10;
    private float m_Knockback = 0.5f;
    private float m_HitSphereRange = 0.2f;

    private Dictionary<string, GameObject> m_AlreadyHit = new Dictionary<string, GameObject>();

    [SerializeField] private Transform m_Attackpoint;

    protected float m_DetectRange = 10f;
    protected float m_AttackRange = 1f;
    protected float m_MoveSpeed = 3f;
    protected float m_TurnTime = 0.2f;
    protected float m_TurnVel;

    //private GameObject m_HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        m_Anim = gameObject.GetComponentInChildren<Animator>();
        m_AnimFlags = gameObject.GetComponentInChildren<EnemyAnimationFlags>();
        m_RigidBody = gameObject.GetComponent<Rigidbody>();
        m_Stats = GetComponent<Stats>();
        m_BoxCollider = GetComponent<BoxCollider>();
        m_PlayerLocation = GameObject.Find("player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
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

    private void FixedUpdate()
    {
        ExecuteState();
    }

    private void SetState(State t_State)
    {
        m_State = t_State;

        switch (m_State)
        {
            case State.idle:
                break;

            case State.walking:
                m_Anim.SetBool("move", true);
                break;

            case State.dead:
                m_Anim.SetTrigger("death");
                m_RigidBody.isKinematic = true;
                m_BoxCollider.enabled = false;
                break;

            case State.combat:
                m_Anim.SetTrigger("attack");
                m_AnimFlags.CombatStart();
                break;

            case State.hit:
                m_Anim.SetTrigger("hit");
                m_AnimFlags.HitStart();
                break;

            default:
                break;
        }
    }

    private void ExecuteState()
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

    public virtual void Idle()
    {
        float distance = Mathf.Abs(Vector3.Distance(m_PlayerLocation.position, transform.position));
        if (distance <= m_DetectRange && distance > m_AttackRange)
        {
            SetState(State.walking);
        }
        else if(distance <= m_AttackRange)
        {
            SetState(State.combat);
        }
        
    }

    public virtual void Walking()
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
            SetState(State.idle);
        }
    }

    public virtual void Hit()
    {
        if(m_AnimFlags.HitStatus())
        {
            SetState(State.idle);
        }
    }

    public virtual void Combat()
    {
        if (m_AnimFlags.CombatHitBox())
        {
            // Detect enemies in range
            List<Collider> hitPlayer = new List<Collider>();
            hitPlayer.AddRange(Physics.OverlapSphere(m_Attackpoint.position, m_HitSphereRange, m_PlayerLayer));



            //Damage/effects enemies
            foreach (Collider player in hitPlayer)
            {
                player.GetComponent<Rigidbody>().AddForce(transform.forward * m_Knockback, ForceMode.Impulse);
                Debug.Log("Hit Player!");
                if (m_AlreadyHit.ContainsKey(player.gameObject.name))
                {
                    continue;
                }

                player.GetComponent<PlayerController>().GotHit(m_AttackDamage);

                m_AlreadyHit.Add(player.gameObject.name, player.gameObject);
            }
        }
        if (!m_AnimFlags.CombatStatus())
        {
            m_AlreadyHit.Clear();
            SetState(State.idle);
        }
    }

    public void GotHit(int t_damage)
    {
        SetState(State.hit);
        m_Stats.Damage(t_damage);
    }
}
