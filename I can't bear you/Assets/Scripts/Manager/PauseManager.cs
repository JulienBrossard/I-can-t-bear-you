using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenu;
    
    private void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseMenu.SetActive(true);
        }
    }
    
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        Application.LoadLevel(sceneName);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
}
