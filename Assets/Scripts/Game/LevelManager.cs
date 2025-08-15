using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Level manager serves as the system for managing transtioning between scenes for my game. 
    public Animator transitionAnimator;
    public float transitionTime = 1f;
    void Start()
    {

    }

    IEnumerator LoadLevelCoroutine(string sceneName)
    {
        // Play the transition animation
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Load");
        }

        // Wait for the transition animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReloadLevel()
    {
        // Reload the current level
        Time.timeScale = 1f; // Ensure the game is unpaused before reloading
        StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().name));
    }

    public void LoadLevel(string sceneName)
    {
        // Load the specified level
        Time.timeScale = 1f; // Ensure the game is unpaused before loading a new level
        StartCoroutine(LoadLevelCoroutine(sceneName));
    }

    public void QuitGame()
    {
        // Quit the game
        Debug.Log("Quitting game...");
        Application.Quit();

        // If running in the editor, stop playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

}
