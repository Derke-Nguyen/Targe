using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public enum ShieldState { thrown, recalled, equipped, stuck, sheathed }

    [SerializeField]
    private ShieldState m_State = ShieldState.equipped;

    private float m_RotationSpeed = 350;

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
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                break;

            case ShieldState.sheathed:
                gameObject.transform.parent = m_Back;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(0,0,0);
                break;

            case ShieldState.stuck:
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
        transform.localEulerAngles += Vector3.forward * m_RotationSpeed * Time.deltaTime;
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
}
