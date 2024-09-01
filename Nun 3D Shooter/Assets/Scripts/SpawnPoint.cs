using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    Wave[] wavesOfEnemies;
    [SerializeField]
    Vector3[] spawnPoints;
 
    private int currentWaveIndex = 0;

    void Start()
    {
        if (wavesOfEnemies.Length > 0)
        {
            Wave currentWave = wavesOfEnemies[currentWaveIndex];
            currentWave.SpawnWave(wavesOfEnemies[currentWaveIndex], spawnPoints);
        }
    }

    void SpawnWaves()
    {
        //WaveManager currentWave = wavesOfEnemies[currentWaveIndex];
        foreach (Wave wave in wavesOfEnemies)
        {
            wave.SpawnWave(wave, spawnPoints);
            while (!wave.IsWaveComplete())
            {
                continue;   
            }
        }
        // if (currentWave.IsWaveComplete())
        // {
        //     currentWaveIndex++;
        //     if (currentWaveIndex < wavesOfEnemies.Length)
        //     {
        //         currentWave.SpawnWave(wavesOfEnemies[currentWaveIndex], spawnPoints);
        //     }
        // }
    }
}
