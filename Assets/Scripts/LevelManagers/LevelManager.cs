using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // int of the scene index
    private int m_LevelNumber;

    // bool if the room is completed
    private bool m_Completed = false;

    // bool if the room is paused
    private bool m_Paused = false;

    // GameObject that is the player
    private GameObject m_Player;

    // gui manager
    private GUIManager m_GUI;

    // input manager
    private InputController input;

    // Start is called before the first frame update
    public virtual void Start()
    {
        m_Player = GameObject.Find("player");
        m_GUI = GameObject.Find("GUI").GetComponent<GUIManager>();
        input = GameObject.Find("InputController").GetComponent<InputController>();
        m_LevelNumber = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (m_Player.GetComponent<Stats>().IsDead())
        {
            m_GUI.DeathScreen();
            return;
        }

        m_Paused = m_GUI.PauseStatus();

        if (input.Menu())
        {
            if (m_Paused)
            {
                m_GUI.Resume();
            }
            else
            {
                m_GUI.Pause();
            }
        }
    }

    public int GetLevelIndex()
    {
        return m_LevelNumber;
    }

    public bool IsCompleted()
    {
        return m_Completed;
    }

    public void SetCompleted(bool isCompleted)
    {
        m_Completed = isCompleted;
    }
}
