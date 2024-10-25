using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ImpEnemy : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    private Wave wave;
    
    private ImpMovement enemyMovement;
    private CapsuleCollider enemyCollider;

    public Animator animatorController { get; set; }

    public bool IsAttacking { get; set; }

    private HealthBar healthBar;

    public float currentMoveSpeed;
    public float currentHealth;
    private float currentDamage;
    private float healthCurrency;
    private int currencyAmount;

    private float frameAttackSpeed = 0.6f;


    void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.maxHealth = enemyData.MaxHealth;
        healthBar.healthSlider.maxValue = enemyData.MaxHealth;
        enemyCollider = GetComponent<CapsuleCollider>();
        animatorController = GetComponent<Animator>();
        currentMoveSpeed = enemyData.MoveSpeed;
        enemyMovement = GetComponentInParent<ImpMovement>();
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
        enemyMovement.ShowFloatingText(dmg);
        healthBar.ReduceHealth(dmg);

        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            StartCoroutine(Kill());
        }

        enemyMovement.enabled = false;
        StartCoroutine(Stagger());
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
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);

    }
    private void Update()
    {
        frameAttackSpeed -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            IsAttacking = true;
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage, transform.position, 0); // use currentDamage
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (frameAttackSpeed <= 0)
            {
                IsAttacking = true;
                PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
                player.TakeDamage(currentDamage, transform.position, 0); // use currentDamage
                frameAttackSpeed = 0.8f;
            }
        }
    }
}
