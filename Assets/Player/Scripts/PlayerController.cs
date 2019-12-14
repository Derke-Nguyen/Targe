using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Possible States the player can be in
    public enum State { idle, walking, running };

    // Player's current state
    [SerializeField]
    private State m_State = State.idle;

    // Player's animator
    private Animator anim;

    // Input Controller
    private InputController input;

    // Camera's position
    private Transform CameraT;

    /*
     * What happens on start frame
     */
    private void Start()
    {
        anim = this.GetComponentInChildren<Animator>();
        input = GameObject.Find("InputController").GetComponent<InputController>();
        CameraT = Camera.main.transform;
    }

    /*
     * What happens every frame
     */
    private void Update()
    {

    }

    /*
     * What happens every fixed amount of frames
     */
    private void FixedUpdate()
    {
        ExecuteState();
    }

    private void SetState(State t_state)
    {
        m_State = t_state;
    }

    private void ExecuteState()
    {
        switch(m_State)
        {
            case State.idle:
                anim.SetBool("IsWalking", false);
                Idle();
                break;

            case State.walking:
                anim.SetBool("IsWalking", true);
                Walking();
                break;

            default:
                break;
        }
    }

    private void Idle()
    {
        if (input.Vert() != 0 || input.Hori() != 0)
        {
            SetState(State.walking);
        }
    }

    private void Walking()
    {
        if(input.Vert() == 0 && input.Hori() == 0)
        {
            SetState(State.idle);
        }
    }

}
