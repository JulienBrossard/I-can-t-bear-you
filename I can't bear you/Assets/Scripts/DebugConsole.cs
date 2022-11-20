using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugConsole : MonoBehaviour
{ 
    void Start()
    {
        Debug.developerConsoleVisible = true;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Debug.developerConsoleVisible = !Debug.developerConsoleVisible;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
