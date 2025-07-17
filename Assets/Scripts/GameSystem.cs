using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class EnemyType
{
    public string name; // Name of the enemy type
    public GameObject prefab; // Prefab of the enemy to be spawned
    public float minimumWaveLevel; // Minimum wave level for this enemy type
    public float maximumWaveLevel; // Maximum wave level for this enemy type
    public float spawnChance; // Chance of this enemy type spawning in a wave
}

public class GameSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //i love the video game


    public EnemyType[] enemyTypes; // Array of enemy types to spawn
    public int maxEnemyCount = 10;
    //private int currentEnemyCount = 0;
    public float spawnInterval = 2f;

    public float spawnRadius = 5f; // Radius around the level center to spawn enemies

    public Transform levelCenter;

    public int currentWaveLevel = 1; // Current wave level, can be used to scale difficulty
    private int _currentWaveLevel
    {
        get { return currentWaveLevel; }
        set
        {
            currentWaveLevel = Mathf.Max(1, value); // Ensure wave level is at least 1
        }
    }

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Generate a random position within a defined area
        float x = UnityEngine.Random.Range(-spawnRadius, spawnRadius);
        float y = UnityEngine.Random.Range(-spawnRadius, spawnRadius);
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
            //grabn a new enemy type based on the current wave level

            EnemyType enemyType = Array.Find(enemyTypes, e => e.minimumWaveLevel <= _currentWaveLevel && e.maximumWaveLevel >= _currentWaveLevel && UnityEngine.Random.value < e.spawnChance);
            if (enemyType != null)
            {
                GameObject enemy = Instantiate(enemyType.prefab, GetRandomSpawnPosition(), Quaternion.identity);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
        
    }
    // Update is called once per frame
    void Update()
        {

        }
}
