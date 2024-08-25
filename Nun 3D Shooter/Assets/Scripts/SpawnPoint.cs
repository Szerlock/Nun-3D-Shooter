using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    GameObject[] enemies;
    
    [SerializeField]
    Vector3[] spawnPoints;

    [SerializeField]
    private int enemiesPerMinute;

    private float spawnIntervals;

    [SerializeField]
    private float timer;


    void Start()
    {
        spawnIntervals = 60f / enemiesPerMinute; // Calculate spawn interval
        timer = spawnIntervals; // Initialize the timer
    }

    void Update()
    {
        timer -= Time.deltaTime; // Decrease the timer by the time passed since the last frame
        
        if (timer <= 0f)
        {
            timer = spawnIntervals; // Reset the timer
            SpawnEnemy(); // Spawn an enemy
        }
    }

    private void SpawnEnemy()
    {

        Vector3 spawnPosition = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        
        // Instantiate the enemy at the chosen spawn point
        GameObject newEnemy = Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length)]);
        newEnemy.transform.position = spawnPosition;

    }
}
