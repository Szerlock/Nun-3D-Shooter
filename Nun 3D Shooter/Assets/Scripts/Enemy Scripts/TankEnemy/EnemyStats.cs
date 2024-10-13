using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    private Wave wave;
    private BoxCollider enemyCollider;

    [SerializeField]
    public GameObject showTextDamage;
    private TankMovement enemyMovement;

    public bool IsAttacking { get; set; }

    public float currentMoveSpeed;
    private float currentHealth;
    private float currentDamage;
    private float healthCurrency;
    private int currencyAmount;

    void Awake()
    {
        enemyCollider = GetComponent<BoxCollider>();
        currentMoveSpeed = enemyData.MoveSpeed;
        enemyMovement = GetComponent<TankMovement>();
        IsAttacking = false;
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

        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            StartCoroutine(Kill());
        }

        enemyMovement.enabled = false;
        StartCoroutine(Stagger());
    }

    private void ShowFloatingText(float dmg)
    {
        Transform cameraTransform = Camera.main.transform;

        var go = Instantiate(showTextDamage, transform.position, Quaternion.LookRotation(transform.position - cameraTransform.position), transform);
        go.GetComponent<TextMesh>().text = dmg.ToString();
    }

    private IEnumerator Stagger()
    {
        yield return new WaitForSeconds(1f);
        enemyMovement.enabled = true;
    }

    private IEnumerator Kill()
    {
        enemyCollider.enabled = false;
        yield return new WaitForSeconds(0.3f);

        wave.EnemyDied(currencyAmount);
        CurrencyManager.Instance.AddHealthCurrency((int)System.Math.Round(healthCurrency));
        Destroy(gameObject);
    }

    private void OnCollisionStay(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            IsAttacking = true;
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage, transform.position, 4); // use currentDamage
        }
    }
}
