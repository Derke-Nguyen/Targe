using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    private bool GameIsPaused = false;
    private bool GameOver = false;
    private InputController input;
    public GameObject pauseMenuUI;
    public GameObject deathScreen;

    // Start is called before the first frame update
    void Start()
    {
        input = GameObject.Find("InputController").GetComponent<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(input.Menu() && !GameOver)
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        //Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        //Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void DeathScreen()
    {
        deathScreen.SetActive(true);
        GameOver = true;
    }

    private void Retry()
    {
        GameOver = false;
        deathScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
