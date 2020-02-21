/**
 * File: MenuButton.cs 
 * Author: Derek Nguyen
 * 
 * Parent class for a MenuButton
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    //Menu Button Controller
    [SerializeField] private ButtonController controller;
    // Button's Animator
    [SerializeField] private Animator m_Anim;
    // Flags of animation
    [SerializeField] private AnimatorFunctions m_AnimFuncs;
    // Index of the button
    [SerializeField] private int m_Index = 0;

    private bool m_Selected;

    /** 
     * What happens on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_AnimFuncs = GetComponent<AnimatorFunctions>();
    }

    /**
     * What happens every frame
     * 
     * If menu index is this button, play selected animation
     * If button is pressed, set button to active and play it
     * Reset button otherwise
     */
    void Update()
    {
        if(controller.GetIndex() == m_Index)
        {
            m_Anim.SetBool("selected", true);

            if (Input.GetButtonDown("Submit"))
            {
                m_Anim.SetBool("pressed", true);
            }
        }
        else
        {
            m_Anim.SetBool("selected", false);
            m_Anim.SetBool("pressed", false);
        }
    }

    /**
     * what happens when the button is pressed
     * 
     * Changes to next level
     */
    public virtual void ButtonEffects()
    {
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToNextLevel();
    }

}
