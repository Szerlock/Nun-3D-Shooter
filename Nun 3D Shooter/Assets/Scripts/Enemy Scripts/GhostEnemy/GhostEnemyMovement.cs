using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemyMovement : MonoBehaviour
{
    public float runSpeed; // Speed at which the enemy will run
    public float detectionRange; // Range within which the enemy detects the player
    private Transform playerTransform; // Reference to the player's transform
    public GhostEnemyStats ghostEnemyStats;

    public float attackCooldown = 2f;

    private float minY;
    private float maxY;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }
        minY = transform.position.y;
        maxY = transform.position.y;
    }

    private void Update()
    {
        //if (playerTransform == null) return;

        //float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        //if (distanceToPlayer <= detectionRange)
        //{
        //    Vector3 directionAwayFromPlayer = (transform.position - playerTransform.position).normalized;
        //    transform.position += directionAwayFromPlayer * runSpeed * Time.deltaTime;

        //    gameObject.transform.LookAt(playerTransform);
        //}

        //if (attackCooldown <= 0)
        //{
        //    ghostEnemyStats.ProjectileFire(playerTransform.position);
        //    attackCooldown = 2f;
        //}
        //else
        //{
        //    attackCooldown -= Time.deltaTime;
        //}

        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Calculate direction away from the player
            Vector3 directionAwayFromPlayer = (transform.position - playerTransform.position).normalized;

            // Calculate the new position
            Vector3 newPosition = transform.position + directionAwayFromPlayer * runSpeed * Time.deltaTime;

            // Clamp the Y position to stay within defined bounds
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            // Update the enemy's position
            transform.position = newPosition;

            // Make the enemy look at the player
            gameObject.transform.LookAt(playerTransform);
        }

        // Projectile attack logic (cooldown management)
        if (attackCooldown <= 0)
        {
            ghostEnemyStats.ProjectileFire(playerTransform.position);
            attackCooldown = 2f;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }
}
