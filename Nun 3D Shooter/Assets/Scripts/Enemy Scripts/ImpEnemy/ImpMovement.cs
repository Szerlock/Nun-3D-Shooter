using System;
using System.Collections;
using UnityEngine;

public class ImpMovement : MonoBehaviour
{
    ImpEnemy enemy;
    Transform player;
    Rigidbody rb;


    void Start()
    {
        enemy = GetComponentInChildren<ImpEnemy>();
        player = FindObjectOfType<PlayerMovement>().transform;
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemy.IsAttacking)
        {
            transform.LookAt(player.transform);

            // Move towards the player using the rigidbody
            Vector3 newPosition = Vector3.MoveTowards(rb.position, player.position, enemy.currentMoveSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);
        }
        else
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);
        yield return new WaitForSeconds(random.NextFloat(2f, 8f));
        enemy.IsAttacking = false;
    }
}
