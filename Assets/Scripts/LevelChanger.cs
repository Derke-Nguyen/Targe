/**
 * File: PlayerController.cs 
 * Author: Derek Nguyen
 * 
 * Class responsible for changing the level
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    // animator to handle scene transitions
    public Animator m_Anim;
    private int m_LevelToLoad;

    /**
     * What happesn on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /**
     * Fades screen to next level
     * 
     * Gets current build index and goes one up
     */
    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /**
     * Fades screen to chosen level
     * 
     * t_LevelIndex : the level to load
     */
    public void FadeToLevel(int t_LevelIndex)
    {
        m_LevelToLoad = t_LevelIndex;
        m_Anim.SetTrigger("FadeOut");
    }

    /**
     * When fade finishes, move to next level
     */
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(m_LevelToLoad);
    }

    /**
     * When quitting application
     */
    public void ToQuit()
    {
        Application.Quit();
    }

    /**
     * when resetting level, just load current level
     */
    public void Reset()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex);
    }
}
