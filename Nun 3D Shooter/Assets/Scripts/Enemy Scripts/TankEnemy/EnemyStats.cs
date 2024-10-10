using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    [SerializeField]
    private Wave wave;

    [SerializeField]
    public ShowTextDamage showTextDamage;

    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    public int healthCurrency;
    [HideInInspector]
    public int currencyAmount;

    void Awake()
    {
        healthCurrency = enemyData.HealthCurrencyAmount;
        currentMoveSpeed = enemyData.MoveSpeed;
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
    }

    private IEnumerator Kill()
    {
        yield return new WaitForSeconds(0.3f);

        wave.EnemyDied(currencyAmount);
        Destroy(gameObject);
    }

    private void OnCollisionStay(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage, transform.position); // use currentDamage
        }
    }
}
