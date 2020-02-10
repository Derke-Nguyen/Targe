using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    public enum BossState { idle, walking, hit, combat, dead };

    private BossState m_BossState = BossState.idle;

    private float m_FootRange = 0.2f;
    private int m_FootDamage = 5;
    [SerializeField] private Transform m_Footpoint;

    private int m_HardDamage = 25;
    private float m_HeavyKnockback = 2f;

    private bool m_Defeated;

    private Dictionary<string, GameObject> m_AlreadyHit = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        m_AttackDamage = 15;
        m_Knockback = 1f;
        m_HitSphereRange = 0.8f;


        m_DetectRange = 30f;
        m_AttackRange = 2f;
        m_MoveSpeed = 2f;
        m_TurnTime = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        //No character control if player is already dead
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
                m_BoxCollider.enabled = false;
                break;

            case BossState.combat:
                m_Anim.SetTrigger("lattack");
                m_AnimFlags.CombatStart();
                break;

            case BossState.hit:
                m_Anim.SetTrigger("hit");
                m_AnimFlags.HitStart();
                break;

            default:
                break;
        }
    }

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
                base.Hit();
                break;

            case BossState.dead:
                break;

            case BossState.combat:
                Combat();
                break;

            default:
                break;
        }
    }

    protected override void Idle()
    {
        Vector3 target = m_PlayerLocation.position - transform.position;
        target.Normalize();
        float targetRot = Mathf.Acos(Vector3.Dot(target, transform.forward));
        bool facing = (targetRot < 0.3) ? true : false;
        if (!facing)
        {
            Vector2 targetLocation = new Vector2(target.x, target.z);
            float targetLook = Mathf.Atan2(targetLocation.x, targetLocation.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetLook, ref m_TurnVel, m_TurnTime);
        }
        float distance = Mathf.Abs(Vector3.Distance(m_PlayerLocation.position, transform.position));
        if (distance <= m_DetectRange && distance > m_AttackRange)
        {
            SetState(BossState.walking);
        }
        else if (distance <= m_AttackRange && facing)
        {
            SetState(BossState.combat);
        }

    }

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

    protected override void Hit()
    {
        if (m_AnimFlags.HitStatus())
        {
            SetState(BossState.idle);
        }
    }

    protected override void Combat()
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
                if (m_AlreadyHit.ContainsKey(player.gameObject.name))
                {
                    continue;
                }

                player.GetComponent<PlayerController>().GotHit(m_AttackDamage, true);

                m_AlreadyHit.Add(player.gameObject.name, player.gameObject);
            }
        }
        if (!m_AnimFlags.CombatStatus())
        {
            m_AlreadyHit.Clear();
            float distance = Mathf.Abs(Vector3.Distance(m_PlayerLocation.position, transform.position));
            Vector3 target = m_PlayerLocation.position - transform.position;
            target.Normalize();
            float targetRot = Mathf.Acos(Vector3.Dot(target, transform.forward) / (distance));
            if (distance <= m_AttackRange && targetRot == 0)
            {
                SetState(BossState.combat);
            }
            else
            {
                SetState(BossState.idle);
            }
        }
    }

    public override void GotHit(int t_damage, bool m_unblockable = false)
    {
        base.GotHit(t_damage, m_unblockable);
        SetState(State.hit);
        m_Stats.Damage(t_damage);
    }

    private void OnDrawGizmosSelected()
    {
        if (m_Attackpoint == null || m_Footpoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(transform.position, m_AttackRange);
        Gizmos.DrawWireSphere(m_Attackpoint.position, m_HitSphereRange);
        Gizmos.DrawWireSphere(m_Footpoint.position, m_FootRange);
    }
}
