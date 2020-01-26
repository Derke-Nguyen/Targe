using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    public Slider m_HPBar;
    public Stats m_Health;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Health = GameObject.Find("player").GetComponent<Stats>();
        m_HPBar = GetComponent<Slider>();
        m_HPBar.maxValue = 100;
        m_HPBar.minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_HPBar.value = m_Health.GetHealth();
    }
}
