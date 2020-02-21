/**
 * File: BossHPBar.cs 
 * Author: Derek Nguyen
 * 
 * Manager for boss health bar
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    // The main health bar that is drawn
    public Slider m_HPBar;
    // The aftereffect that follows the main healthbar
    public Slider m_HPBarAfter;
    // The Boss's health status
    [SerializeField]
    Stats m_Health;

    /* What happesn on start frame
     * 
     * Gathers all components that are needed and initializes the object
     */
    void Start()
    {
        m_HPBar = GetComponent<Slider>();
        m_HPBarAfter = transform.Find("HealthBarAfterEffect").GetComponent<Slider>();
        m_HPBar.maxValue = m_Health.GetMaxHealth();
        m_HPBar.minValue = 0;
        m_HPBarAfter.maxValue = m_HPBar.maxValue;
        m_HPBarAfter.minValue = 0;
        m_HPBar.value = m_Health.GetHealth();
        m_HPBarAfter.value = 1000;
    }

    /* What happens every frame
     * 
     * Sets health bar's value as actual heatlh
     * If boss is dead, set it to inactive
     */
    void Update()
    {
        m_HPBar.value = m_Health.GetHealth();
        if(m_Health.IsDead())
        {
            gameObject.SetActive(false);
        }
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
