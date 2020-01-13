using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour
{
    private Stats m_Player;
    GUIManager m_GUIManager;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.Find("player").GetComponent<Stats>();
        m_GUIManager = GameObject.Find("GUI").GetComponent<GUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            m_Player.SetCurrHealth(0);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (m_Player.IsDead())
        {
            m_GUIManager.DeathScreen();
        }
    }
}
