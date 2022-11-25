using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugControl : MonoBehaviour
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
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            BearserkerGaugeManager.instance.AddBearserker(1);
        }
    }
}
