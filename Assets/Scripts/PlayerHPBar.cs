/**
 * File: PlayerHPBar.cs 
 * Author: Derek Nguyen
 * 
 * Manager for player health bar
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    // The main health bar that is drawn
    public Slider m_HPBar;
    // The aftereffect that follows the main healthbar
    public Slider m_HPBarAfter;
    // The Boss's health status
    public Stats m_Health;

    /* What happesn on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    void Start()
    {
        m_Health = GameObject.Find("player").GetComponent<Stats>();
        m_HPBar = GetComponent<Slider>();
        m_HPBarAfter = transform.Find("HealthBarAfterEffect").GetComponent<Slider>();
        m_HPBar.maxValue = m_Health.GetMaxHealth();
        m_HPBar.minValue = 0;
        m_HPBarAfter.maxValue = m_HPBar.maxValue;
        m_HPBarAfter.minValue = m_HPBar.minValue;
        m_HPBar.value = m_Health.GetHealth();
        m_HPBarAfter.value = m_HPBar.value;
    }

    /* What happens every frame
     * 
     * Sets health bar's value as actual heatlh
     */
    void Update()
    {
        m_HPBar.value = m_Health.GetHealth();
    }

    /* What happens every fixed amount of frames
     * 
     * Makes aftereffect follow actual healthbar
     */
    private void FixedUpdate()
    {
        if (m_HPBarAfter.value > m_HPBar.value)
        {
            m_HPBarAfter.value -= 0.2f;
        }
    }
}
