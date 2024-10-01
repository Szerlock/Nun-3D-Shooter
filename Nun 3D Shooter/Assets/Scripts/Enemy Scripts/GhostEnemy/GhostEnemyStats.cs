using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    private Wave wave;
    public GameObject projectile;
    public Transform projectileSpawnPoint;
    public Transform playerTransform;

    public float currentHealth;
    public float currentDamage;

    void Awake()
    {
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    public void SetEnemyData(Wave wave)
    {
        this.wave = wave;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if(currentHealth <= 0)
        {
            Kill();
        }
    }

    public void ProjectileFire()
    {
        Vector3 directionToPlayer = (playerTransform.position - projectileSpawnPoint.position).normalized;
        // Instantiate the bullet at the bullet spawn point
        GameObject projectileGameObject = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // Set bullet velocity in the direction the player is looking
        Rigidbody rb = projectileGameObject.GetComponent<Rigidbody>();
        rb.velocity = directionToPlayer * enemyData.ProjectileSpeed;

        // Set bullet damage
        GhostProjectile ghostProjectile = projectileGameObject.GetComponent<GhostProjectile>();
        if (ghostProjectile != null)
        {
            ghostProjectile.SetDamage(currentDamage);
        }
        else
        {
            Debug.LogError("Bullet script not found on bullet prefab.");
        }
    }

    private void Kill()
    {
        Destroy(gameObject);
        //wave.EnemyDied();
    }

    private void OnCollisionStay(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage); // use currentDamage
        }
    }
}
