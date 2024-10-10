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
    [SerializeField]
    public ShowTextDamage showTextDamage;

    public float currentHealth;
    public float currentDamage;

    void Awake()
    {
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    public void SetWave(Wave wave)
    {
        this.wave = wave;
    }

    public void TakeDamage(float dmg)
    {
        StartCoroutine(showTextDamage.ShowDamage(dmg, transform));

        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            StartCoroutine(Kill());
        }
    }

    private IEnumerator Kill()
    {
        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
        wave.EnemyDied(1);
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
}
