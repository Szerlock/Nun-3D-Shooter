using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public List<EnemySpawnData> enemySpawnData;
    public GameObject Boss;
    public Vector3 bossSpawnPoint;

    private int amountPerEnemy;
    public int enemiesAlive = 0;
    Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);


    public void SpawnWave(Wave wave, List<Vector3> spawnPoints)
    {
        if (Boss != null && bossSpawnPoint != null) 
        {
            GameObject newEnemy = Instantiate(Boss, bossSpawnPoint, Quaternion.identity);
            newEnemy.GetComponent<DoubleFaceStats>().SetWave(this);
            enemiesAlive++;
        }
        foreach (EnemySpawnData enemyData in enemySpawnData)
        {
            amountPerEnemy = enemyData.GetQuantity();
            GameObject prefab = enemyData.enemyPrefab;
            for (int i = 0; i < amountPerEnemy; i++)    
            {
                GameObject newEnemy = Instantiate(prefab);
                Vector3 spawnPosition = spawnPoints[random.NextInt(0, spawnPoints.Count)];
                newEnemy.transform.position = spawnPosition;

                if(newEnemy.CompareTag("Exploding_Enemy"))
                {
                    newEnemy.GetComponent<ExplodingEnemyStats>().SetWave(this);
                }
                else if(newEnemy.CompareTag("Tank_Enemy"))
                {
                    newEnemy.GetComponent<EnemyStats>().SetWave(this);
                }
                else if(newEnemy.CompareTag("Imp_Enemy"))
                {
                    newEnemy.GetComponentInChildren<ImpEnemy>().SetWave(this);
                }
                else if(newEnemy.CompareTag("Ghost_Enemy"))
                {
                    newEnemy.GetComponent<GhostEnemyStats>().SetWave(this);
                }
                enemiesAlive++;
                StartCoroutine(SpawnEnemy());
            }
        }
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(random.NextFloat(0, 2));
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
