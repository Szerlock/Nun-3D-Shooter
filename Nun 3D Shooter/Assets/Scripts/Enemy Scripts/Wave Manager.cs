using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public EnemySpawnData[] enemySpawnData;
    public float interval;
    public int enemiesAlive = 0;
    
    public void SpawnWave(Wave waveManager, Vector3[] spawnPoints)
    {        
        // Instantiate the enemy at the chosen spawn point
        for(int i = 0; i < waveManager.enemySpawnData.Length; i++)
        {
            Vector3 spawnPosition = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            GameObject newEnemy = Instantiate(waveManager.enemySpawnData[i].enemyPrefab);
            newEnemy.transform.position = spawnPosition;
            enemiesAlive++;
        }
    }

    public bool IsWaveComplete()
    {
        if(enemiesAlive == 0)
        {
            return true;
        }
        return false;
    }

    public void EnemyDied()
    {
        enemiesAlive--;
    }
}
