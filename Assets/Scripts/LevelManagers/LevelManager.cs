/**
 * File: LevelManager.cs 
 * Author: Derek Nguyen
 * 
 * Base Class for LevelManager
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // int of the scene index
    private int m_LevelNumber;

    // bool if the room is completed
    protected bool m_Completed = false;

    // bool if the room is paused
    private bool m_Paused = false;

    // GameObject that is the player
    private PlayerController m_Player;

    // gui manager
    public GUIManager m_GUI;

    // input manager
    public InputController input;

    /**
     * What happesn on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    public virtual void Start()
    {
        m_Player = GameObject.Find("player").GetComponent<PlayerController>();
        m_GUI = GameObject.Find("GUI").GetComponent<GUIManager>();
        input = GameObject.Find("InputController").GetComponent<InputController>();
        m_LevelNumber = SceneManager.GetActiveScene().buildIndex;
    }

    /**
     * What happens every frame
     * 
     * If player is dead, send to GUI
     * If paused, but GUI is not active, recheck statuses
     * If menu button is pressed, pause the game
     */
    public virtual void Update()
    {
        if (m_Player.GetComponent<Stats>().IsDead())
        {
            m_GUI.DeathScreen();
            return;
        }
        //If level is paused, but GUI doesn't match
        if(m_Paused != m_GUI.PauseStatus())
        {
            m_Paused = m_GUI.PauseStatus();
            //Pause if GUI is active
            if(m_Paused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        if (input.Menu())
        {
            if (m_Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    /**
     * Get the level's current index in build
     * return : level's index in build
     */
    public int GetLevelIndex()
    {
        return m_LevelNumber;
    }

    /**
     * If the level is completed, tell other ojects
     * return : if level is completed or not
     */
    public bool IsCompleted()
    {
        return m_Completed;
    }

    /**
     * If the level is paused, set GUI and pause player
     */
    public virtual void Pause()
    {
        m_GUI.Pause();
        m_Player.Pause();
    }

    /**
     * If the level is unpaused, set GUI and unpause player
     */
    public virtual void Resume()
    {
        m_GUI.Resume();
        m_Player.Resume();
    }
}
