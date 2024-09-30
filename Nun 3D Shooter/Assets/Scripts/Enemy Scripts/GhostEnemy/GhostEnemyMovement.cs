using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
    }
}
