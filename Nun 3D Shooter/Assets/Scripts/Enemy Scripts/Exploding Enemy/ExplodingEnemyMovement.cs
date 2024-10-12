using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : MonoBehaviour
{
    ExplodingEnemyStats enemy;
    Transform player;
    Rigidbody rb;

    void Start()
    {
        enemy = GetComponent<ExplodingEnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!enemy.IsExploding()){
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.LookAt(player.transform);

        Vector3 newPosition = Vector3.MoveTowards(rb.position, player.position, enemy.currentMoveSpeed * Time.deltaTime);

        rb.MovePosition(newPosition);
    }
}
