using System;
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
    public GameObject showTextDamage;

    private BoxCollider enemyCollider;
    private HealthBar healthBar;

    [HideInInspector]
    public float currentMoveSpeed;
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    [HideInInspector]
    public float healthCurrency;
    public int currencyAmount;

    void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.maxHealth = enemyData.MaxHealth;
        healthBar.healthSlider.maxValue = enemyData.MaxHealth;
        enemyCollider = GetComponent<BoxCollider>();
        healthCurrency = enemyData.HealthCurrencyAmount;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        currencyAmount = enemyData.CurrencyAmount;
    }

    public void SetWave(Wave wave)
    {
        this.wave = wave;
    }

    public void TakeDamage(float dmg)
    {
        ShowFloatingText(dmg);
        healthBar.ReduceHealth(dmg);

        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            StartCoroutine(Kill());
        }
    }

    private void ShowFloatingText(float dmg)
    {
        Transform cameraTransform = Camera.main.transform;

        var go = Instantiate(showTextDamage, transform.position, Quaternion.LookRotation(transform.position - cameraTransform.position), transform);
        go.GetComponent<TextMesh>().text = dmg.ToString();
    }

    private IEnumerator Kill()
    {
        enemyCollider.enabled = false;

        yield return new WaitForSeconds(0.3f);

        wave.EnemyDied(currencyAmount);
        CurrencyManager.Instance.AddHealthCurrency((int)System.Math.Round(healthCurrency));
        Destroy(gameObject);

    }

    public void ProjectileFire(Vector3 targetPosition)
    {
        Vector3 directionToPlayer = (targetPosition - projectileSpawnPoint.position).normalized;

        GameObject projectileGameObject = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);

        Rigidbody rb = projectileGameObject.GetComponent<Rigidbody>();
        rb.velocity = directionToPlayer * enemyData.ProjectileSpeed;

        GhostProjectile ghostProjectile = projectileGameObject.GetComponent<GhostProjectile>();
        if (ghostProjectile != null)
        {
            ghostProjectile.SetDamage(currentDamage);
        }
        else
        {
            Debug.LogError("Projectile script not found on projectile prefab.");
        }
    }
}
