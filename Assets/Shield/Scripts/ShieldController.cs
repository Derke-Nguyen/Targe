/**
 * File: ShieldController.cs 
 * Author: Derek Nguyen
 * 
 * Controls the shield
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    // Possible States the shield can be in
    public enum ShieldState { thrown, recalled, equipped, stuck }

    // The current state of the shield
    [SerializeField]
    private ShieldState m_State = ShieldState.equipped;

    // How fast the shield rotates while it flies
    private float ROTATION_SPEED = 350;
    // The force that pushes the shield
    private float THROW_POWER = 40;

    // Reference to the rigidbody of the shield to interact with physics
    private Rigidbody m_RigidBody;
    // Reference to the collider for collision physics
    private BoxCollider m_Collider;

    // If the shield is in the player's hands
    private bool m_InHand = false;

    // Damage that the shield deals
    private int m_Damage = 10;

    // The point that the shield curves towards
    [SerializeField]
    private Transform m_CurvePoint;

    // The shield's current position
    [SerializeField]
    private Transform m_Position;

    // The hand position that the shield will fly towards
    [SerializeField]
    private Transform m_Hand;

    // The start position when the shield is pulled
    [SerializeField]
    private Vector3 m_PullPosition;

    // The shield's original Local position
    private Vector3 m_OrigLocPos;

    // The sheild's original Rotation 
    private Vector3 m_OrigLocRot;
    
    // How long it's taking for the shield to return
    private float m_ReturnTime = 0;

    /** 
     * What happens on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<BoxCollider>();
        m_CurvePoint = GameObject.Find("CurvePoint").GetComponent<Transform>();
        m_Hand = GameObject.Find("ShieldPoint").GetComponent<Transform>();
    }

    /** 
     * What happens every frame
     * 
     * Executes the current state
     */
    void Update()
    {
        ExecuteState();
    }

    /** 
     * Sets the shield's state
     * 
     * t_State : the state that the shield will be set as
     */
    private void SetState(ShieldState t_State)
    {
        m_State = t_State;
        m_InHand = false;
        m_Collider.enabled = true;

        switch (t_State)
        {
            // orphans the shield from player's hand
            case ShieldState.thrown:
                gameObject.transform.parent = null;
                break;

            // Sets position to fly back
            case ShieldState.recalled:
                m_ReturnTime = 0;
                m_RigidBody.isKinematic = true;
                m_PullPosition = transform.position;
                break;

            // Places shield at player's hand
            case ShieldState.equipped:
                m_InHand = true;
                gameObject.transform.parent = m_Hand;
                m_RigidBody.isKinematic = true;
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                m_Collider.enabled = false;
                break;

            case ShieldState.stuck:
                m_RigidBody.isKinematic = true;
                break;

            default:
                break;
        }

    }

    /** 
     * Executes shield instructions based on what state the player is in
     */
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

    /**
     * Shield action for when it is thrown
     */
    private void Thrown()
    {
        transform.localEulerAngles += Vector3.forward * ROTATION_SPEED * Time.deltaTime;
    }

    /**
     * Shield actions for when it is recalled
     * 
     * if the return time is less than one, have it keep flying
     */
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
            SetState(ShieldState.equipped);
        }

    }

    /**
     * Shield action when it's equipped, probably do nothing
     */
    private void Equipped()
    {

    }
    /**
     * When shield collides with something
     */
    private void OnTriggerEnter(Collider other)
    {
        // Basically ignore if it collides with the camera or the player
        if (other.gameObject.tag == "MainCamera" || other.gameObject.tag == "Player")
        {
            return;
        }
        // Ignore any spcial level cases
        else if(other.gameObject.tag == "Level")
        {
            return;
        }
        if (m_State == ShieldState.recalled)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyController>().GotHit(m_Damage);
            }
                return;
        }
        // If collides with enemy during flying, ignore, else deal damage and freeze them
        else if (m_State == ShieldState.thrown)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyController>().Frozen();
            }
            other.gameObject.GetComponent<EnemyController>().GotHit(m_Damage);
        }  
        SetState(ShieldState.stuck);
    }

    /**
     * When shield is recalled and leaves the enemy hitbox, unfreeze
     */
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

    /**
     * Throws the shield in a certain direction
     * 
     * t_Direction : direction that the shield will be thrown in
     */
    public void Thrown(Vector3 t_Direction)
    {
        SetState(ShieldState.thrown);
        m_RigidBody.isKinematic = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
        m_RigidBody.AddForce(t_Direction * THROW_POWER + transform.up * 2, ForceMode.Impulse);
    }

    /**
     * Checks if shield is in the hand
     * 
     * return : bool if shield is in hand
     */
    public bool InHand()
    {
        return m_InHand;
    }

    /**
     * Gets the current point for the shield along the curve based on the time 
     * 
     * t_ReturnTime : current return time
     * t_StartPoint : Where the point started
     * t_CurvePoint : The current point that the shield is on
     * t_FinalPOint : The final target point
     */
    private Vector3 GetQuadraticCurvePoint(float t_ReturnTime, Vector3 t_StartPoint, Vector3 t_CurvePoint, Vector3 t_FinalPoint)
    {
        float timeleft = 1 - t_ReturnTime;
        float time_square = t_ReturnTime * t_ReturnTime;
        float timeleft_square = timeleft * timeleft;
        return (timeleft_square * t_StartPoint) + (2 * timeleft * t_ReturnTime * t_CurvePoint) + (time_square * t_FinalPoint);
    }

    /**
     * Execute if the shield is stuck, maybe do nothing
     */
    private void Stuck()
    {

    }

    /**
     * If the shield starts recall, starts moving it back
     */
    public void StartRecall()
    {
        SetState(ShieldState.recalled);
    }
}
