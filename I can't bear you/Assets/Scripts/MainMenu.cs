using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void LaunchLevel(String lvlToLoad)
    {
        SceneManager.LoadScene(lvlToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
