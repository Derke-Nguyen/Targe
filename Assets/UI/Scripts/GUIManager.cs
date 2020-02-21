using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    // If the game is paused
    private bool m_GamePaused = false;

    // The pause menu
    public GameObject m_PauseMenu;
    // The death menu
    public GameObject m_DeathScreen;
    // Reticle for aiming
    public GameObject m_Reticle;

    /**
     * If game is unpaused
     */
    public void Resume()
    {
        m_PauseMenu.SetActive(false);
        m_GamePaused = false;
    }

    /**
     * If game is paused
     */
    public void Pause()
    {
        m_PauseMenu.SetActive(true);
        m_GamePaused = true;
    }

    /**
     * If game is paused 
     * 
     * return: if game is paused, true
     */
    public bool PauseStatus()
    {
        return m_GamePaused;
    }

    /**
     * If player has died, set death screen
     */
    public void DeathScreen()
    {
        m_DeathScreen.SetActive(true);
        m_DeathScreen.GetComponent<Animator>().SetBool("dead", true);
        gameObject.GetComponent<AudioSource>().Pause();
    }

    /**
     * If game is paused
     */
    private void Retry()
    {
        m_DeathScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /**
     * If player is aiming shield
     */
    public void AimOn()
    {
        m_Reticle.SetActive(true);
    }

    /**
     * If player is no longer aiming shield
     */
    public void AimOff()
    {
        m_Reticle.SetActive(false);
    }
}
