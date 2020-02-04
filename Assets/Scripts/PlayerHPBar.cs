using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    public Slider m_HPBar;
    public Slider m_HPBarAfter;
    public Stats m_Health;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Health = GameObject.Find("player").GetComponent<Stats>();
        m_HPBar = GetComponent<Slider>();
        m_HPBarAfter = transform.Find("HealthBarAfterEffect").GetComponent<Slider>();
        m_HPBar.maxValue = m_HPBar.maxValue;
        m_HPBar.minValue = 0;
        m_HPBarAfter.maxValue = m_HPBar.maxValue;
        m_HPBarAfter.minValue = m_HPBar.minValue;
        m_HPBar.value = m_Health.GetHealth();
        m_HPBarAfter.value = m_HPBar.value;
    }

    // Update is called once per frame
    void Update()
    {
        m_HPBar.value = m_Health.GetHealth();
    }

    private void FixedUpdate()
    {
        if (m_HPBarAfter.value != m_HPBar.value)
        {
            m_HPBarAfter.value -= 0.01f;
        }
    }
}
