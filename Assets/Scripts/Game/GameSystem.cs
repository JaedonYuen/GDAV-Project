using System.Collections;
using System;

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
    // Game system: handles spawning
    // Spawning is done first by grabbing a valid location and then instantiating the enemy prefab at that location.
    // An enemy is chosen by grabbing from the available enemy types based on their spawn chance and the current wave level.
    // The system ensures that the number of active enemies does not exceed the maximum limit.
    // The limit of how many is spawn increases based of the wave level, so the higher the wave, the more enemies there is, making the game harder


    public EnemyType[] enemyTypes; // Array of enemy types to spawn
    public int maxEnemyCount = 10; //private int currentEnemyCount = 0;
    public float spawnInterval = 2f;
    public Vector2 spawnIntervalRange = new Vector2(1f, 5f);

    public Transform mapTopLeft;
    public Transform mapBottomRight; // these transforms form a rectangle of the spawn area

    public Transform levelCenter;

    public GameObject barrier;

    public GameObject bossEnemy; // Only had time for one boss, since i had to make the assets myself.

    private bool waveActive = false; // Flag to check if a wave is currently active


    private int _currentWaveLevel = 0; // Current wave level, starts at 0

    public int currentWaveLevel
    {
        get { return _currentWaveLevel; }
    }

    void Start()
    {
        
    }

    
    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(UnityEngine.Random.Range(mapTopLeft.position.x, mapBottomRight.position.x), UnityEngine.Random.Range(mapTopLeft.position.y, mapBottomRight.position.y), 0);
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
        for (int i = 0; i < (maxEnemyCount+((maxEnemyCount/2)*_currentWaveLevel)); i++)
        {

            // Check if the wave is a multiple of 4 and check if this is the final enemy spawned in
            if (_currentWaveLevel % 4 == 0 && i == (maxEnemyCount + ((maxEnemyCount / 2) * _currentWaveLevel)) - 1)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                Instantiate(bossEnemy, spawnPosition, Quaternion.identity);
            }
            else
            {
                // Choose a random enemy type based on spawn chance and wave level
                EnemyType enemyType = chooseEnemyType();
                if (enemyType != null)
                {
                    Vector3 spawnPosition = GetRandomSpawnPosition();
                    Instantiate(enemyType.prefab, spawnPosition, Quaternion.identity);
                }
            }
            


                // Wait for the spawn interval before spawning the next set of enemies
                yield return new WaitForSeconds(UnityEngine.Random.Range(spawnIntervalRange.x, spawnIntervalRange.y));
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
