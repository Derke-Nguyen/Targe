using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    Slider m_HPBar;
    Slider m_HPBarAfter;
    [SerializeField]
    Stats m_Health;
    
    // Start is called before the first frame update
    void Start()
    {
        m_HPBar = GetComponent<Slider>();
        m_HPBarAfter = transform.Find("HealthBarAfterEffect").GetComponent<Slider>();
        m_HPBar.maxValue = m_Health.GetMaxHealth();
        m_HPBar.minValue = 0;
        m_HPBarAfter.maxValue = m_HPBar.maxValue;
        m_HPBarAfter.minValue = 0;
        m_HPBar.value = m_Health.GetHealth();
        m_HPBarAfter.value = m_HPBar.value;
    }

    // Update is called once per frame
    void Update()
    {
        m_HPBar.value = m_Health.GetHealth();
        if(m_Health.IsDead())
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (m_HPBarAfter.value > m_HPBar.value)
        {
            m_HPBarAfter.value -= 0.2f;
        }
    }
}
