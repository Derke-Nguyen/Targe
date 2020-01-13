using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonController : MonoBehaviour
{
    private int m_Index;
    [SerializeField] private bool m_KeyDown;
    [SerializeField] int m_MaxIndex;
    public AudioSource audio;
    public LevelChanger lc;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeyDown();
    }

    private void CheckKeyDown()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!m_KeyDown)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (m_Index < m_MaxIndex)
                    {
                        m_Index++;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (m_Index > 0)
                    {
                        m_Index--;
                    }
                }
                m_KeyDown = true;
            }
        }
        else
        {
            m_KeyDown = false;
        }
    }

    public int GetIndex()
    {
        return m_Index;
    }

    public void PlayEffect()
    {
        if (m_Index == m_MaxIndex)
        {
            Application.Quit();
        }
        else
        {
            lc.OnFadeComplete();
        }
    }
}
