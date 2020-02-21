/**
 * File: LevelMover.cs 
 * Author: Derek Nguyen
 * 
 * Class for changing levels between levels
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMover : MonoBehaviour
{
    //The level changer
    private LevelChanger m_LevelChanger;

    /**
     * What happesn on start frame
     * 
     * Gets current build index
     */
    private void Start()
    {
        m_LevelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
    }

    /**
     * What happesn when player enters the collider
     * 
     * transitions to next level
     */
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            m_LevelChanger.FadeToNextLevel();
        }
    }
}
