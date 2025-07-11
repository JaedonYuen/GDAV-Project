using System.Collections;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //i love the video game
    // this is a really early prototype of the game loop 
    //it litreally only spawns enemies.

    public GameObject enemyPrefab;
    public int maxEnemyCount = 10;
   //private int currentEnemyCount = 0;
    public float spawnInterval = 2f;

    public float spawnRadius = 5f; // Radius around the level center to spawn enemies

    public Transform levelCenter; 

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Generate a random position within a defined area
        float x = Random.Range(-spawnRadius, spawnRadius);
        float y = Random.Range(-spawnRadius, spawnRadius);
        return new Vector3(x, y, 0) + levelCenter.position; // Assuming y=0 for ground level
    }

    private int countEnemies()
    {
        // Count the number of active enemies in the scene
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    IEnumerator SpawnEnemies()
    {
        while (countEnemies() < maxEnemyCount)
        {
            GameObject enemy = Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
