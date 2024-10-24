using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemyMovement : MonoBehaviour
{
    public float runSpeed = 0.005f; // Speed at which the enemy will run
    public float detectionRange = 10f; // Range within which the enemy detects the player
    private Transform playerTransform; // Reference to the player's transform
    public GhostEnemyStats ghostEnemyStats;

    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    private void Start()
    {
        // Assuming player has a tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // Check the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // If the player is within detection range, run away
        if (distanceToPlayer <= detectionRange)
        {
            // Run away logic
            Vector3 directionAwayFromPlayer = (transform.position - playerTransform.position).normalized;
            transform.position += directionAwayFromPlayer * runSpeed * Time.deltaTime;

            // Make the enemy look at the player
            gameObject.transform.LookAt(playerTransform);
        }

        // Projectile attack logic (cooldown management)
        if (attackCooldown <= 0)
        {
            ghostEnemyStats.ProjectileFire();
            attackCooldown = 2f;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }
}
