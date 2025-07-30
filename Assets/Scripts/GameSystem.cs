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
    public int maxEnemyCount = 10; //private int currentEnemyCount = 0;
    public float spawnInterval = 2f;
    public float waveInterval = 5f; // Variance in spawn interval to add randomness

    public float spawnRadius = 5f; // Radius around the level center to spawn enemies

    public Transform levelCenter;

    public GameObject barrier;

    private bool waveActive = false; // Flag to check if a wave is currently active


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

        barrier.SetActive(true);
        waveActive = true;
        // Reset the current wave level
        _currentWaveLevel++;

        // Spawn enemies based on the current wave level
        for (int i = 0; i < maxEnemyCount; i++)
        {
            // Choose a random enemy type based on spawn chance and wave level
            EnemyType enemyType = chooseEnemyType();
            if (enemyType != null)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                Instantiate(enemyType.prefab, spawnPosition, Quaternion.identity);
            }

            // Wait for the spawn interval before spawning the next set of enemies
            yield return new WaitForSeconds(spawnInterval);
        }
        barrier.SetActive(false);
        waveActive = false;

    }

    EnemyType chooseEnemyType()
    {
        float totalChance = 0f;
        foreach (var enemyType in enemyTypes)
        {
            if (_currentWaveLevel >= enemyType.minimumWaveLevel && _currentWaveLevel <= enemyType.maximumWaveLevel)
            {
                totalChance += enemyType.spawnChance;
            }
        }

        float randomValue = UnityEngine.Random.Range(0, totalChance);
        float cumulativeChance = 0f;

        foreach (var enemyType in enemyTypes)
        {
            if (_currentWaveLevel >= enemyType.minimumWaveLevel && _currentWaveLevel <= enemyType.maximumWaveLevel)
            {
                cumulativeChance += enemyType.spawnChance;
                if (randomValue < cumulativeChance)
                {
                    return enemyType;
                }
            }
        }

        return null; // No suitable enemy type found
    }

    public void StartWave()
    {
        if (!waveActive)
        {
            StartCoroutine(SpawnEnemies());
        }
    }
    
}
