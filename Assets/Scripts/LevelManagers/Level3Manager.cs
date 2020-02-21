/**
 * File: Level3Manager.cs 
 * Author: Derek Nguyen
 * 
 * Override for LevelManager
 * LevelManager for level 3
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Manager : LevelManager
{
    // GameObject that is the portal
    private GameObject m_Gate;

    // Lists of all the enemies
    [SerializeField]
    private List<GameObject> m_Grunts = new List<GameObject>();
    [SerializeField]
    private List<GameObject> m_Casters = new List<GameObject>();
    [SerializeField]
    private List<GameObject> m_Heavies = new List<GameObject>();

    // Current stage of the level
    private int m_Stage = 0;

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
    }

    /**
     * What happens every fixed amount of frames
     * 
     * If enemy is dead, remove from list of enemies
     */
    private void FixedUpdate()
    {
        if(m_Completed)
        {
            m_Gate.SetActive(true);
        }
        else
        {
            switch (m_Stage)
            {
                case 0:
                    foreach (GameObject grunt in m_Grunts)
                    {
                        if (grunt.GetComponent<Stats>().IsDead())
                        {
                            m_Grunts.Remove(grunt);
                            break;
                        }
                    }
                    // If there are no more grunts, move onto next stage
                    if (m_Grunts.Count == 0)
                    {
                        m_Stage++;
                        foreach (GameObject caster in m_Casters)
                        {
                            caster.SetActive(true);
                        }
                    }
                    break;
                case 1:
                    foreach (GameObject caster in m_Casters)
                    {
                        if (caster.GetComponent<Stats>().IsDead())
                        {
                            m_Casters.Remove(caster);
                            break;
                        }
                    }
                    // If there are no more casters, move onto next stage
                    if (m_Casters.Count == 0)
                    {
                        foreach (GameObject heavy in m_Heavies)
                        {
                            heavy.SetActive(true);
                        }
                        m_Stage++;
                    }
                    break;
                case 2:
                    foreach (GameObject heavy in m_Heavies)
                    {
                        if (heavy.GetComponent<Stats>().IsDead())
                        {
                            m_Heavies.Remove(heavy);
                            break;
                        }
                    }
                    // If there are no more heavies, unlock portal
                    if (m_Heavies.Count == 0)
                    {
                        m_Stage++;
                        m_Completed = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    /**
     * If the level is paused, pause all enemies
     */
    public override void Pause()
    {
        base.Pause();
        switch (m_Stage)
        {
            case 0:
                foreach (GameObject grunt in m_Grunts)
                {
                    grunt.GetComponent<EnemyController>().Pause();
                }
                break;
            case 1:
                foreach (GameObject caster in m_Casters)
                {
                    caster.GetComponent<EnemyController>().Pause();
                }
                break;
            case 2:
                foreach (GameObject heavy in m_Heavies)
                {
                    heavy.GetComponent<EnemyController>().Pause();
                }
                break;
            default:
                break;
        }
    }

    /**
     * If the level is unpaused, activate all enemies
     */
    public override void Resume()
    {
        base.Resume();
        switch (m_Stage)
        {
            case 0:
                foreach (GameObject grunt in m_Grunts)
                {
                    grunt.GetComponent<EnemyController>().Resume();
                }
                break;
            case 1:
                foreach (GameObject caster in m_Casters)
                {
                    caster.GetComponent<EnemyController>().Resume();
                }
                break;
            case 2:
                foreach (GameObject heavy in m_Heavies)
                {
                    heavy.GetComponent<EnemyController>().Resume();
                }
                break;
            default:
                break;
        }
    }
}
