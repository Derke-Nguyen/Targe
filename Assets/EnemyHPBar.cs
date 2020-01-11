using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    Slider m_HPBar;
    Stats m_Health;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Health = transform.parent.parent.GetComponent<Stats>();
        m_HPBar = GetComponent<Slider>();
        m_HPBar.maxValue = 100;
        m_HPBar.minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_HPBar.value = m_Health.GetHealth();
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
