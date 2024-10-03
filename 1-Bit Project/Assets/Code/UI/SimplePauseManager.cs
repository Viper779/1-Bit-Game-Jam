using UnityEngine;

public class SimplePauseManager : MonoBehaviour
{
    public static SimplePauseManager Instance { get; private set; }

    private bool isPaused = false;

    // Reference to your pause menu UI
    public GameObject pauseMenuUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Check for Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        // Activate or deactivate the pause menu UI
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(isPaused);
        }

        Debug.Log(isPaused ? "Game Paused" : "Game Resumed");
    }

    public bool IsGamePaused()
    {
        return isPaused;
        
    }
}
