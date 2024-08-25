using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    GameObject[] enemies;
    void Start()
    {
        Instantiate(enemies[Random.Range(0, enemies.Length)]).transform.position = transform.position;
    }

}
