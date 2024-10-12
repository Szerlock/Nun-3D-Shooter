using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpEnemy : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    private Wave wave;
    [SerializeField]
    public ShowTextDamage showTextDamage;
    private ImpMovement enemyMovement;

    public bool IsAttacking { get; set; }

    public float currentMoveSpeed;
    private float currentHealth;
    private float currentDamage;
    private float healthCurrency;
    private int currencyAmount; 

    void Awake()
    {
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
        StartCoroutine(showTextDamage.ShowDamage(dmg, transform));

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
        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
        wave.EnemyDied(currencyAmount);
    }

    private void OnCollisionStay(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            IsAttacking = true;
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage, transform.position, 0); // use currentDamage
        }
    }
}
