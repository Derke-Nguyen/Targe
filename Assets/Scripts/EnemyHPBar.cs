/**
 * File: EnemyHPBar.cs 
 * Author: Derek Nguyen
 * 
 * Manager for enemy health bar
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    // The main health bar that is drawn
    Slider m_HPBar;
    // The aftereffect that follows the main healthbar
    Slider m_HPBarAfter;
    // The Boss's health status
    Stats m_Health;

    /* What happesn on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    void Start()
    {
        m_Health = transform.parent.parent.GetComponent<Stats>();
        m_HPBar = GetComponent<Slider>();
        m_HPBarAfter = transform.Find("HealthBarAfterEffect").GetComponent<Slider>();
    }

    /* What happens every frame
     * 
     * Sets health bar's value as actual heatlh
     * If enemy is dead, set it to inactive
     */
    void Update()
    {
        m_HPBar.value = m_Health.GetHealth();
        if (m_Health.IsDead())
        {
            gameObject.SetActive(false);
        }
    }

    /* What happens every fixed amount of frames
     * 
     * Makes aftereffect follow actual healthbar
     * Rotates enemy healthbar to camera
     */
    private void FixedUpdate()
    {
        if (m_HPBarAfter.value > m_HPBar.value)
        {
            m_HPBarAfter.value -= 0.2f;
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}
