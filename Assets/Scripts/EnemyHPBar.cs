using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    Slider m_HPBar;
    Slider m_HPBarAfter;
    Stats m_Health;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Health = transform.parent.parent.GetComponent<Stats>();
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
        if (m_HPBarAfter.value > m_HPBar.value)
        {
            m_HPBarAfter.value -= 0.2f;
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
