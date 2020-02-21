/**
 * File: Level2Manager.cs 
 * Author: Derek Nguyen
 * 
 * Override for LevelManager
 * LevelManager for level 2
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : LevelManager
{
    // GameObject that is the portal
    private GameObject m_Gate;

    // List of all the enemies
    [SerializeField]
    private List<GameObject> m_Enemies = new List<GameObject>();

    /**
     * What happesn on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    public override void Start()
    {
        base.Start();

        m_Gate = GameObject.Find("Gate");
        m_Gate.SetActive(false);
        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            m_Enemies.Add(thing);
        }
    }

    /**
     * What happens every frame
     * 
     * If no enemies, set the level to completed
     */
    public override void Update()
    {
        base.Update();

        if (m_Enemies.Count == 0 && !m_Completed)
        {
            m_Completed = true;
            m_Gate.SetActive(true);
        }
    }

    /**
     * What happens every fixed amount of frames
     * 
     * If enemy is dead, remove from list of enemies
     */
    private void FixedUpdate()
    {
        if(!m_Completed)
        {
            foreach (GameObject enemy in m_Enemies)
            {
                if (enemy.GetComponent<Stats>().IsDead())
                {
                    m_Enemies.Remove(enemy);
                    break;
                }
            }
        }
    }

    /**
     * If the level is paused, pause all enemies
     */
    public override void Pause()
    {
        base.Pause();
        foreach (GameObject enemy in m_Enemies)
        {
            enemy.GetComponent<EnemyController>().Pause();
        }
    }

    /**
     * If the level is unpaused, activate all enemies
     */
    public override void Resume()
    {
        base.Resume();
        foreach (GameObject enemy in m_Enemies)
        {
            enemy.GetComponent<EnemyController>().Resume();
        }
    }
}
