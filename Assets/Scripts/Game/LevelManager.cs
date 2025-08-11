using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void reloadLevel()
    {
        // Reload the current level
        Time.timeScale = 1f; // Ensure the game is unpaused before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
