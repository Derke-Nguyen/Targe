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

    //For Recall
    [SerializeField]
    private Transform m_CurvePoint;
    [SerializeField]
    private Transform m_Position;
    [SerializeField]
    private Transform m_Hand;
    [SerializeField]
    private Transform m_Back;

    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {

            SetState(ShieldState.equipped);
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            SetState(ShieldState.sheathed);
        }
        ExecuteState();
    }

    private void SetState(ShieldState t_State)
    {
        m_State = t_State;
        switch (t_State)
        {
            case ShieldState.thrown:
                gameObject.transform.parent = null;
                break;

            case ShieldState.recalled:
                break;

            case ShieldState.equipped:
                gameObject.transform.parent = m_Hand;
                m_RigidBody.isKinematic = true;
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                break;

            case ShieldState.sheathed:
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

    }

    private void Equipped()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(m_State == ShieldState.thrown)
        {
            SetState(ShieldState.stuck);
        }
    }

    public void Thrown(Vector3 t_Direction)
    {
        SetState(ShieldState.thrown);
        m_RigidBody.isKinematic = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
        m_RigidBody.AddForce(t_Direction * THROW_POWER + transform.up * 2, ForceMode.Impulse);
    }
}
