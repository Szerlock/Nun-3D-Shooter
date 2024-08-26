using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    WaveManager[] wavesOfEnemies;
    [SerializeField]
    Vector3[] spawnPoints;
    [SerializeField]
    private float timer;
    //private float numberOfMinutes = 0f;

    private int currentWaveIndex = 0;
    private int currentEnemyTypeIndex = 0;
    private int enemiesSpawned = 0;

    void Start()
    {
        if (wavesOfEnemies.Length > 0)
        {
            StartWave(wavesOfEnemies[currentWaveIndex]);
        }
    }

    void Update()
    {
        if (currentWaveIndex < wavesOfEnemies.Length)
        {
            timer -= Time.deltaTime; // Decrease the timer by the time passed since the last frame

            WaveManager currentWave = wavesOfEnemies[currentWaveIndex];
            EnemySpawnData currentEnemyData = currentWave.enemySpawnData[currentEnemyTypeIndex];

            if (timer <= 0f)
            {
                if (enemiesSpawned < currentEnemyData.quantity)
                {
                    SpawnEnemy(currentEnemyData.enemyPrefab);
                    timer = currentWave.interval; // Reset the timer for the next enemy spawn
                    enemiesSpawned++;
                }
                else
                {
                    // Move to the next enemy type in the current wave
                    currentEnemyTypeIndex++;

                    if (currentEnemyTypeIndex >= currentWave.enemySpawnData.Length)
                    {
                        // Move to the next wave
                        currentWaveIndex++;
                        currentEnemyTypeIndex = 0;

                        if (currentWaveIndex < wavesOfEnemies.Length)
                        {
                            StartWave(wavesOfEnemies[currentWaveIndex]); // Start the next wave
                        }
                    }

                    enemiesSpawned = 0;
                }
            }
        }
    }

    private void StartWave(WaveManager wave)
    {
        // Initialize the timer for the current wave
        timer = wave.interval;
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPosition = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        
        // Instantiate the enemy at the chosen spawn point
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.transform.position = spawnPosition;
    }


    // void Start()
    // {
    //     spawnIntervals = 60f / enemiesPerMinute; // Calculate spawn interval
    //     timer = spawnIntervals; // Initialize the timer
    // }

    // void Update()
    // {
    //     numberOfMinutes += Time.deltaTime; // Increase the number of minutes passed
    //     timer -= Time.deltaTime; // Decrease the timer by the time passed since the last frame
        
    //     if (numberOfMinutes >= 15 * 60f)
    //     {
    //     enemiesPerMinute *= 2;
    //     spawnIntervals = 60f / enemiesPerMinute;
    //     numberOfMinutes = 0f; // Reset the timer to prevent further doubling
    //     }

    //     if (timer <= 0f)
    //     {
    //         timer = spawnIntervals; // Reset the timer
    //         SpawnEnemy(); // Spawn an enemy
    //         numberOfMinutes++; // Increase the number of minutes passed
    //     }
    // }

    // private void SpawnEnemy()
    // {

    //     Vector3 spawnPosition = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        
    //     // Instantiate the enemy at the chosen spawn point
    //     GameObject newEnemy = Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length)]);
    //     newEnemy.transform.position = spawnPosition;

    // }
}
