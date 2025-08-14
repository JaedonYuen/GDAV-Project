using UnityEngine;

public class PlayerPauseHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PauseMenuSystem pauseMenu;

    // Hello. If you are wondering: why the heck is this under the player? well, unity's input system is uber weird and only likes having one player input handler, so this goes in to the player. 

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
