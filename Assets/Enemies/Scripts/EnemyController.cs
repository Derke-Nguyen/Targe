using System.Collections;
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

    //private GameObject m_HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        m_Anim = gameObject.GetComponentInChildren<Animator>();
        m_AnimFlags = gameObject.GetComponentInChildren<EnemyAnimationFlags>();
        m_RigidBody = gameObject.GetComponent<Rigidbody>();
        m_Stats = GetComponent<Stats>();
        m_BoxCollider = GetComponent<BoxCollider>();
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
                m_Anim.SetBool("walking", true);
                break;

            case State.dead:
                m_Anim.SetTrigger("death");
                m_RigidBody.isKinematic = true;
                m_BoxCollider.enabled = false;
                break;

            case State.combat:
                m_Anim.SetTrigger("attack");
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

    }

    public virtual void Walking()
    {

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

    }

    public void GotHit(int t_damage)
    {
        SetState(State.hit);
        m_Stats.Damage(t_damage);
    }
}
