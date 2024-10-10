using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    public List<Vector3> spawnPoints;
 
    public void SpawnWave(Wave wave)
    {
        wave.SpawnWave(wave, spawnPoints);
    }
}
