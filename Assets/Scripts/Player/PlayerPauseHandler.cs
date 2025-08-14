using UnityEngine;

public class PlayerPauseHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PauseMenuSystem pauseMenu;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPause()
    {
        Debug.Log("Application paused");
        if (pauseMenu != null)
        {
            pauseMenu.TogglePause();
        }
        else
        {
            Debug.LogWarning("Pause menu is not assigned in the inspector.");
        }
    }
}
