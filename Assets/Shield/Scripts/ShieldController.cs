using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public enum ShieldState { thrown, recalled, equipped, stuck, sheathed }

    [SerializeField]
    private ShieldState m_State = ShieldState.equipped;

    private float ROTATION_SPEED = 350;
    private float THROW_POWER = 40;

    private Rigidbody m_RigidBody;
    private BoxCollider m_Collider;

    private bool m_InHand = false;
    private bool m_OnBack = false;

    private int m_Damage = 10;

    //For Recall
    [SerializeField]
    private Transform m_CurvePoint;
    [SerializeField]
    private Transform m_Position;
    [SerializeField]
    private Transform m_Hand;
    [SerializeField]
    private Transform m_Back;
    [SerializeField]
    private Vector3 m_PullPosition;
    private Vector3 m_OrigLocPos;
    private Vector3 m_OrigLocRot;
    private float m_ReturnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<BoxCollider>();
        m_CurvePoint = GameObject.Find("CurvePoint").GetComponent<Transform>();
        m_Hand = GameObject.Find("ShieldPoint").GetComponent<Transform>();
        m_Back = GameObject.Find("SheathPoint").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        ExecuteState();
    }

    private void SetState(ShieldState t_State)
    {
        m_State = t_State;
        m_InHand = false;
        m_OnBack = false;
        m_Collider.enabled = true;

        switch (t_State)
        {
            case ShieldState.thrown:
                gameObject.transform.parent = null;
                break;

            case ShieldState.recalled:
                m_RigidBody.isKinematic = true;
                m_PullPosition = transform.position;
                break;

            case ShieldState.equipped:
                m_InHand = true;
                gameObject.transform.parent = m_Hand;
                m_RigidBody.isKinematic = true;
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                m_Collider.enabled = false;
                break;

            case ShieldState.sheathed:
                m_OnBack = true;
                gameObject.transform.parent = m_Back;
                m_RigidBody.isKinematic = true;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(0,0,0);
                break;

            case ShieldState.stuck:
                m_RigidBody.isKinematic = true;
                break;

            default:
                break;
        }

    }

    private void ExecuteState()
    {
        switch(m_State)
        {
            case ShieldState.thrown:
                Thrown();
                break;

            case ShieldState.recalled:
                Recalled();
                break;

            case ShieldState.equipped:
                Equipped();
                break;

            case ShieldState.stuck:
                Stuck();
                break;

            default:
                break;
        }
    }

    private void Thrown()
    {
        transform.localEulerAngles += Vector3.forward * ROTATION_SPEED * Time.deltaTime;
    }

    private void Recalled()
    {
        if(m_ReturnTime < 1)
        {
            transform.position = GetQuadraticCurvePoint(m_ReturnTime, m_PullPosition, m_CurvePoint.position, m_Hand.position);
            m_ReturnTime += Time.deltaTime * 1.5f;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 0), 1);
        }
        else
        {
            m_ReturnTime = 0;
            SetState(ShieldState.equipped);
        }

    }

    private void Equipped()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCamera" || other.gameObject.tag == "Player")
        {
            return;
        }
        else if(other.gameObject.tag == "Level")
        {
            return;
        }
        else if (m_State == ShieldState.recalled)
        {
            if(other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyController>().GotHit(m_Damage);
            }
            return;
        }  
        else if(m_State == ShieldState.thrown && other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().GotHit(m_Damage);
            other.gameObject.GetComponent<EnemyController>().Frozen();
        }
        SetState(ShieldState.stuck);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if(m_State == ShieldState.stuck)
            {
                m_RigidBody.isKinematic = false;
            }
            other.gameObject.GetComponent<EnemyController>().UnFrozen();
        }
    }

    public void Thrown(Vector3 t_Direction)
    {
        SetState(ShieldState.thrown);
        m_RigidBody.isKinematic = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
        m_RigidBody.AddForce(t_Direction * THROW_POWER + transform.up * 2, ForceMode.Impulse);
    }

    public bool InHand()
    {
        return m_InHand;
    }

    public bool OnBack()
    {
        return m_OnBack;
    }

    private Vector3 GetQuadraticCurvePoint(float t_m_ReturnTime, Vector3 t_StartPoint, Vector3 t_CurvePoint, Vector3 t_FinalPoint)
    {
        float timeleft = 1 - t_m_ReturnTime;
        float time_square = t_m_ReturnTime * t_m_ReturnTime;
        float timeleft_square = timeleft * timeleft;
        return (timeleft_square * t_StartPoint) + (2 * timeleft * t_m_ReturnTime * t_CurvePoint) + (time_square * t_FinalPoint);
    }

    private void Stuck()
    {

    }

    public void StartRecall()
    {
        SetState(ShieldState.recalled);
    }
}
