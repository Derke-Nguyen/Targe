using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Input Controller
 * Takes in Input and sets variables to their current active or inactive states
 * Every Scene Should have this */
public class InputController : MonoBehaviour
{
    // Direction left joystick tilted horizontally
    private float m_Horizontal = 0f;
    // Direction Left Joystick tilted vertically 
    private float m_Vertical = 0f;

    // Direction right joystick tilted horizontal
    private float m_CameraHorizontal = 0f;
    // Direction right joystick tilted vertically
    private float m_CameraVertical = 0f;

    // Melee button is pressed
    private bool m_Melee = false;

    // Block button is pressed
    private bool m_Block = false;

    // Dodge button is pressed
    private bool m_Dodge = false;

    // Aim button is pressed
    private bool m_Aim = false;

    // Recall button is pressed
    private bool m_Recall = false;

    // LockOn button is pressed
    private bool m_LockOn = false;

    // Run button is pressed
    private bool m_Run = false;

    // Menu button is pressed
    private bool m_Menu = false;

    /* What happens every frame
     * 
     * Sets variables based on which buttons are being pressed
     */
    void Update()
    {
        m_Vertical = Input.GetAxis("Vertical");
        m_Horizontal = Input.GetAxis("Horizontal");

        m_CameraHorizontal = Input.GetAxis("camHorizontal");
        m_CameraVertical = Input.GetAxis("camVertical");

        m_Aim = Input.GetAxis("Aim") == 0 ? false : true;

        m_Block = Input.GetButton("Block");

        m_Melee = Input.GetButtonDown("Melee");
        m_Dodge = Input.GetButtonDown("Dodge");
        m_Recall = Input.GetButtonDown("Recall");
        m_LockOn = Input.GetButtonDown("LockOn");
        m_Run = Input.GetButtonDown("Run");
        m_Menu = Input.GetButtonDown("Menu");
    }

    /* Returns status of left joystick tilted vertically
     * 
     * return : true if left joystick tilted vertically, else false
     */
    public float Vert()
    {
        return m_Vertical;
    }

    /* Returns status of left joystick tilted horizontally
     * 
     * return : true if left joystick tilted horizontally, else false
     */
    public float Hori()
    {
        return m_Horizontal;
    }

    /* Returns status of right joystick tilted vertically
     * 
     * return : true if right joystick tilted vertically, else false
     */
    public float CamVert()
    {
        return m_CameraVertical;
    }

    /* Returns status of right joystick tilted horizontally
     * 
     * return : true if right joystick tilted horizontally, else false
     */
    public float CamHori()
    {
        return m_CameraHorizontal;
    }

    /* Returns status of melee button
     * 
     * return : true if melee button is pressed, else false
     */
    public bool Melee()
    {
        return m_Melee;
    }

    /* Returns status of block button
     * 
     * return : true if block button is pressed, else false
     */
    public bool Block()
    {
        return m_Block;
    }

    /* Returns status of dodge button
     * 
     * return : true if dodge button is pressed, else false
     */
    public bool Dodge()
    {
        return m_Dodge;
    }

    /* Returns status of aim button
     *  
     * return : true if aim button is pressed, else false
     */
    public bool Aim()
    {
        return m_Aim;
    }

    /* Returns status of recall button
     * 
     * return : true if recall button is pressed, else false
     */
    public bool Recall()
    {
        return m_Recall;
    }

    /* Returns status of lock on button
     * 
     * return : true if lock on button is pressed, else false
     */
    public bool LockOn()
    {
        return m_LockOn;
    }

    /* Returns status of run button
     * 
     * return : true if run button is pressed, else false
     */
    public bool Run()
    {
        return m_Run;
    }

    /* Returns status of menu button
     * 
     * return : true if menu button is pressed, else false
     */
    public bool Menu()
    {
        return m_Menu;
    }

    /* Returns status of buttons that can cancel block
     * 
     * return : true if a block cancel button is pressed, else false
     */
    public bool BlockCancel()
    {
        return m_Horizontal != 0f || m_Vertical != 0f || m_Dodge || m_Melee;
    }

    /* Returns status of buttons that can cancel aim
     * 
     * return : true if an aim cancel button is pressed, else false
     */
    public bool AimCancel()
    {
        return m_Dodge || m_Block;
    }
}
