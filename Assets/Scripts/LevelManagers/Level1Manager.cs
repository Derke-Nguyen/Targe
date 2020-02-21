/**
 * File: Level1Manager.cs 
 * Author: Derek Nguyen
 * 
 * Override for LevelManager
 * LevelManager for level 1
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : LevelManager
{
    // Trigger for when enemies appear
    private BoxCollider m_RoomTrigger;

    // GameObject that is the portal
    private GameObject m_Gate;

    // List of all the enemies in level
    [SerializeField]
    private List<GameObject> m_Enemies = new List<GameObject>();

    // If the level trigger has been activated
    private bool m_Triggered;


    /**
     * What happesn on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    public override void Start()
    {
        base.Start();

        m_RoomTrigger = GetComponent<BoxCollider>();
        m_Gate = GameObject.Find("Gate");
        m_Gate.SetActive(false);
        //Gather all enemies and hides them
        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            m_Enemies.Add(thing);
            thing.SetActive(false);
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

        if(m_Enemies.Count == 0 && !m_Completed)
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
     * What happens when something enters the trigger
     * 
     * If it's the player, makes all enemies appear
     */
    public void OnTriggerEnter(Collider other)
    {
        if(!m_Triggered)
        {
            if (other.tag == "Player")
            {
                m_Triggered = true;
                foreach (GameObject enemy in m_Enemies)
                {
                    enemy.SetActive(true);
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
        if(m_Triggered)
        {
            foreach (GameObject enemy in m_Enemies)
            {
                enemy.GetComponent<EnemyController>().Resume();
            }
        }
    }
}
