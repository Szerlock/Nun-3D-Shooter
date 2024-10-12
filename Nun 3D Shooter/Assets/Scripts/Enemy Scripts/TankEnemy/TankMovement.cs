using System;
using System.Collections;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemy.IsAttacking)
        {
            transform.LookAt(player.transform);
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
