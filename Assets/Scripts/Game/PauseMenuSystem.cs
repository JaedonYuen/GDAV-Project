using UnityEngine;

public class PauseMenuSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool paused = false;
    public GameObject pauseMenuUI;
    void Start()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogError("Pause Menu UI is not assigned in the inspector.");
        }
        else
        {
            pauseMenuUI.SetActive(false); // Ensure the pause menu is hidden at start
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void TogglePause() // Toggle on or off the pausing of the game. time scale basically manipulates the game speed, so setting it to 0 will pause the game. 
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0f; // Pause the game
            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(true); // Show the pause menu UI
            }
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false); // Hide the pause menu UI
            }
        }
    }
}
