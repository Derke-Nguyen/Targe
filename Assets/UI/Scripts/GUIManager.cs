using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    private bool GameIsPaused = false;
    private bool GameOver = false;
    public GameObject pauseMenuUI;
    public GameObject deathScreen;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        //Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        //Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public bool PauseStatus()
    {
        return GameIsPaused;
    }

    public void DeathScreen()
    {
        deathScreen.SetActive(true);
        deathScreen.GetComponent<Animator>().SetBool("dead", true);
        gameObject.GetComponent<AudioSource>().Pause();
        GameOver = true;
    }

    private void Retry()
    {
        GameOver = false;
        deathScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
