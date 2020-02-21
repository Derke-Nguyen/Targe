/**
 * File: ButtonController.cs 
 * Author: Derek Nguyen
 * 
 * Controller that managers the buttons
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // The button the manager is on
    private int m_Index = 0;
    // Checks if scrolling
    private bool m_KeyDown;
    // Max index of the buttons
    [SerializeField] int m_MaxIndex = 0;

    // The audio source that will play sound
    public AudioSource m_Audio;

    // The level changer
    public LevelChanger m_LevelChanger;

    /**
     * What happesn on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    void Start()
    {
        m_Audio = GetComponent<AudioSource>();    
    }

    /**
     * What happens every frame
     * 
     * Checks what key is pressed and reacts accordingly
     */
    void Update()
    {
        CheckKeyDown();
    }

    /**
     * What happens during key presses frame
     * 
     * Checks what key is pressed and reacts accordingly
     */
    private void CheckKeyDown()
    {
        if(Input.GetAxis("Vertical") != 0)
        {
            //Key pressed is only registered once
            if(!m_KeyDown)
            {
                // Move up or down
                if(Input.GetAxis("Vertical")  < 0)
                {
                    if(m_Index < m_MaxIndex)
                    {
                        m_Index++;
                    }
                }
                else if(Input.GetAxis("Vertical") > 0)
                {
                    if(m_Index > 0)
                    {
                        m_Index--;
                    }
                }
                m_KeyDown = true;
            }
        }
        else
        {
            m_KeyDown = false;
        }
    }

    /**
     * Gets the current index of the controller
     * 
     * return : controller's index
     */ 
    public int GetIndex()
    {
        return m_Index;
    }

    /**
     * Plas effect of the button
     * 
     * return : controller's index
     */
    public void PlayEffect()
    {
        if(m_Index == m_MaxIndex)
        {
            Application.Quit();
        }
        else
        {
            m_LevelChanger.OnFadeComplete();
        }
    }

}
