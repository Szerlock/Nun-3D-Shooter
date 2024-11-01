using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private SpawnPoint spawnPoints;
    [SerializeField]
    private float timeBetweenWaves = 20f;
    private int currentWaveIndex = 0;
    public bool waveInProgress = false;
    public static WaveManager instance { get; private set; }


    void Start()
    {
        instance = this;
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        while(currentWaveIndex < waves.Length)
        {
            waveInProgress = true;
            spawnPoints.SpawnWave(waves[currentWaveIndex]);

            while (!waves[currentWaveIndex].IsWaveComplete())
            {
                yield return null;
            }

            waveInProgress = false;

            //float timeElapsed = 0f;
            //while (timeElapsed < timeBetweenWaves && !waveInProgress)
            //{
            //    Debug.Log("Press Enter to start next wave");
            //    if (Input.GetKeyDown(KeyCode.Return))
            //    {
            //        break;
            //    }


            //    timeElapsed += Time.deltaTime;
            //    yield return null;
            //}
            while (true)
            {
                Debug.Log("Press Enter to start next wave");
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    break;
                }

                yield return null;
            }

            currentWaveIndex++;
        }
    }


}
