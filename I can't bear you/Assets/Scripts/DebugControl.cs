using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugControl : MonoBehaviour
{
    private float fps;
    [SerializeField] private TextMeshProUGUI fpsText;


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
            BearserkerGaugeManager.instance.AddBearserker(1,false);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            fpsText.gameObject.SetActive(!fpsText.gameObject.activeSelf);
        }

        if (Input.GetKey(KeyCode.F4))
        {
            NpcManager.instance.SpawnNpc("Npc (After)");
        }

        if (fpsText.gameObject.activeSelf)
        {
            fps = 1.0f / Time.unscaledDeltaTime;
            fpsText.text = fps.ToString("F0"); 
        }
    }
    

}
