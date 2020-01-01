using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMover : MonoBehaviour
{
    [SerializeField]
    private int m_CurrentLevel;

    private void Start()
    {
        m_CurrentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(m_CurrentLevel + 1);
        }
    }
}
