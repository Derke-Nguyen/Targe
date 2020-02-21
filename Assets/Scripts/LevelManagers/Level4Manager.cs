/**
 * File: Level4Manager.cs 
 * Author: Derek Nguyen
 * 
 * Override for LevelManager
 * LevelManager for level 4
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Manager : LevelManager
{
    // The boss
    [SerializeField]
    private GameObject m_Boss;

    // The victory screen/credits
    public GameObject victoryScreen;

    /**
     * What happesn on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    public override void Start()
    {
        base.Start();

        m_Boss = GameObject.Find("enemy_boss");
    }

    /**
     * What happens every frame
     * 
     * If no enemies, set the level to completed
     */
    public override void Update()
    {
        if (m_Boss.GetComponent<Stats>().IsDead())
        {
            m_GUI.GetComponent<AudioSource>().Pause();
            victoryScreen.SetActive(true);
            victoryScreen.GetComponent<Animator>().SetBool("Win", true);
            //If menu button is pressed, close game
            if (input.Menu())
            {
                Debug.Log("Quit");
                Application.Quit();
            }
        }
        else
        {
            base.Update();
        }


    }

    /**
     * If the level is paused, pause boss
     */
    public override void Pause()
    {
        base.Pause();
        m_Boss.GetComponent<EnemyController>().Pause();
    }

    /**
     * If the level is unpaused, activate boss
     */
    public override void Resume()
    {
        base.Resume();
        m_Boss.GetComponent<EnemyController>().Resume();
    }
}
