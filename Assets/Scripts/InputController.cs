using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    // Direction left joystick tilted horizontally
    private float m_Horizontal = 0f;
    [SerializeField]
    // Direction Left Joystick tilted vertically 
    private float m_Vertical = 0f;

    [SerializeField]
    // Direction right joystick tilted horizontal
    private float m_CameraHorizontal = 0f;
    [SerializeField]
    // Direction right joystick tilted vertically
    private float m_CameraVertical = 0f;

    [SerializeField]
    // Melee button is pressed
    private bool m_Melee = false;

    [SerializeField]
    // Block button is pressed
    private bool m_Block = false;

    [SerializeField]
    // Dodge button is pressed
    private bool m_Dodge = false;

    [SerializeField]
    // Aim button is pressed
    private bool m_Aim = false;

    [SerializeField]
    // Recall button is pressed
    private bool m_Recall = false;

    [SerializeField]
    private bool m_LockOn = false;

    [SerializeField]
    private bool m_Run = false;

    [SerializeField]
    private bool m_Menu = false;

    /*
     * Sets bools of what input is done
     */
    void Update()
    {
        m_Vertical = Input.GetAxis("Vertical");
        m_Horizontal = Input.GetAxis("Horizontal");

        m_CameraHorizontal = Input.GetAxis("camHorizontal");
        m_CameraVertical = Input.GetAxis("camVertical");

        m_Aim = Input.GetAxis("Aim") == 0 ? false : true;

        m_Melee = Input.GetButton("Melee");
        m_Dodge = Input.GetButton("Dodge");
        m_Block = Input.GetButton("Block");
        m_Recall = Input.GetButton("Recall");
        m_LockOn = Input.GetButton("LockOn");
        m_Run = Input.GetButton("Run");
        m_Menu = Input.GetButton("Menu");
    }

    /*
     * return : true if left joystick tilted vertically, else false
     */
    public float Vert()
    {
        return m_Vertical;
    }

    /*
     * return : true if left joystick tilted horizontally, else false
     */
    public float Hori()
    {
        return m_Horizontal;
    }

    /*
     * return : true if right joystick tilted vertically, else false
     */
    public float CamVert()
    {
        return m_CameraVertical;
    }

    /*
     * return : true if right joystick tilted horizontally, else false
     */
    public float CamHori()
    {
        return m_CameraHorizontal;
    }

    /*
     * return : true if melee button is pressed, else false
     */
    public bool Melee()
    {
        return m_Melee;
    }

    /*
      * return : true if block button is pressed, else false
      */
    public bool Block()
    {
        return m_Block;
    }

    /*
      * return : true if dodge button is pressed, else false
      */
    public bool Dodge()
    {
        return m_Dodge;
    }

    /*
      * return : true if aim button is pressed, else false
      */
    public bool Aim()
    {
        return m_Aim;
    }

    /*
      * return : true if recall button is pressed, else false
      */
    public bool Recall()
    {
        return m_Recall;
    }

    /*
      * return : true if lock on button is pressed, else false
      */
    public bool LockOn()
    {
        return m_LockOn;
    }

    /*
      * return : true if run button is pressed, else false
      */
    public bool Run()
    {
        return m_Run;
    }

    /*
      * return : true if menu button is pressed, else false
      */
    public bool Menu()
    {
        return m_Menu;
    }
}
