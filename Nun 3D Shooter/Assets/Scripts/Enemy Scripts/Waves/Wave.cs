using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public List<EnemySpawnData> enemySpawnData;

    private int amountPerEnemy;
    //public float interval;
    public int enemiesAlive = 0;
    
    public void SpawnWave(Wave wave, List<Vector3> spawnPoints)
    {    
        foreach(EnemySpawnData enemyData in enemySpawnData)
        {
            amountPerEnemy = enemyData.GetQuantity();
            GameObject prefab = enemyData.enemyPrefab;
            for (int i = 0; i < amountPerEnemy; i++)    
            {
                GameObject newEnemy = Instantiate(prefab);
                Vector3 spawnPosition = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
                newEnemy.transform.position = spawnPosition;

                if(newEnemy.CompareTag("Exploding_Enemy"))
                {
                    newEnemy.GetComponent<ExplodingEnemyStats>().SetWave(this);
                }
                else if(newEnemy.CompareTag("Tank_Enemy"))
                {
                    newEnemy.GetComponent<EnemyStats>().SetWave(this);
                }
                else if(newEnemy.CompareTag("DoubleFace_Enemy"))
                {
                    newEnemy.GetComponent<DoubleFaceStats>().SetWave(this);
                }
                else if(newEnemy.CompareTag("Imp_Enemy"))
                {
                    newEnemy.GetComponent<EnemyStats>().SetWave(this);
                }
                else if(newEnemy.CompareTag("Ghost_Enemy"))
                {
                    newEnemy.GetComponent<GhostEnemyStats>().SetWave(this);
                }
                enemiesAlive++;
            }
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

    public void EnemyDied(int currencyAmount)
    {
        enemiesAlive--;
        CurrencyManager.Instance.AddCurrency(currencyAmount);
    }
}
