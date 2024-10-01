using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemyMovement : MonoBehaviour
{
     public float runSpeed = 5f; // Speed at which the enemy will run
    private Transform playerTransform; // Reference to the player's transform
    private bool isRunningAway = false; // State if enemy is running away
    public GhostEnemyStats ghostEnemyStats;

    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered Sphere Collider");
            playerTransform = other.transform;
            isRunningAway = true; // Start running away
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Left Sphere Collider");
            isRunningAway = false; // Stop running away
            playerTransform = null; // Clear the player reference
        }
    }

    private void Update()
    {
        if (isRunningAway && playerTransform != null)
        {
            // Calculate direction away from the player
            Vector3 directionAwayFromPlayer = (transform.position - playerTransform.position).normalized;
            // Move the enemy in the direction away from the player
            transform.position += directionAwayFromPlayer * runSpeed * Time.deltaTime;
        }
        else if (playerTransform != null)
        {
            // Check if enough time has passed to fire another projectile
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Debug.Log("Fire Projectile");
                ghostEnemyStats.ProjectileFire();
                lastAttackTime = Time.time; // Reset the attack timer
            }
        }
    }
}
